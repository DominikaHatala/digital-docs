using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Archiver
    {
        #region Fields

        const string m_ConnectionString = "Data Source=.\\Archive.db;Version=3;New=True;";
        string m_SearchedString;

        #endregion

        #region Public Methods

        public Archiver()
        {
            InitializeComponent();
            CreateDataBase();
            for(int i =0;i < 10; i++)
            AddActivity("test", $"testActivity{i}");
        }

        public List<string> GetProcessHistory(string processId)
        {
            var history = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(m_ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {

                    // Select and display database entries
                    command.CommandText = $"Select * FROM Archive WHERE ProcessId='{processId}' ";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add((string)reader["ActivityId"]);
                        }
                    }
                }
                connection.Close(); // Close the connection to the database
            }

            return history;
        }

        public void AddActivity(string processId, string activityId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"INSERT OR IGNORE INTO Archive (ProcessId,ActivityId) VALUES ('{processId}','{activityId}' )";

                    command.ExecuteNonQuery();
                }

                connection.Close(); // Close the connection to the database
            }
        }

        #endregion

        #region Private Methods

        private void ClearDataBase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Create the table
                    command.CommandText = @"DROP TABLE Archive";
 
                    command.ExecuteNonQuery();

                }
                connection.Close(); // Close the connection to the database
            }

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Create the table
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS [Archive] (
                                               [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                               [ProcessId] NVARCHAR(2048)  NULL,
                                               [ActivityId] VARCHAR(2048)  NULL,
                                               UNIQUE(ProcessId, ActivityId)

                                               )";
                    command.ExecuteNonQuery();

                }
                connection.Close(); // Close the connection to the database
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            ListView.ItemsSource = GetProcessHistory(m_SearchedString);
        }

        private void LoginBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            m_SearchedString = LoginBox.Text;
        }

        #endregion

        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new User1_NewTasks();
            newForm.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearDataBase();
        }
    }
}