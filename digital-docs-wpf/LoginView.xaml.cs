using System;
using System.Windows;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView
    {
        string currentUser = "";
        string currentPassword = "";
        string[] passwords = new string[] { "user1", "user2", "user3", "user4" };


        public LoginView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            switch (currentUser)
            {
                case "user1":
                    if (currentPassword == passwords[0])
                    {
                        var newForm = new User1_NewTasks();
                        newForm.Show();
                        XmlTransformer xmlTranformer = new XmlTransformer();
                        Close();
                    } else
                    {
                        MessageBox.Show("Incorrect password");
                    }
                    break;
                default:
                    MessageBox.Show("Incorrect credentails");
                    break;
            }
        }

        private void LoginBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            currentUser = LoginBox.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            currentPassword = PasswordBox.Password;
        }
    }
}