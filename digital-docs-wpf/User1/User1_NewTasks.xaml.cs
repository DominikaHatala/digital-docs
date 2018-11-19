using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace digital_docs_wpf
{
    public partial class User1_NewTasks : Window
    {
        bool[] checkedEmployees = new bool[] { false, false, false }; // employees: 2,3,4

        public String fileName = "";

        public User1_NewTasks()
        {
            InitializeComponent();
        }

        private void Accept_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new ExcelDialog();
            window.Show();

            for (int i = 0 ; i < checkedEmployees.Length; i++)
            {
                if (checkedEmployees[i])
                {
                    Mail mail = new Mail();
                    mail.send(i+2, fileName, checkedEmployees.Length);
                }
            }
        }

        private void Attachment_OnClick(object sender, RoutedEventArgs e)
        {
            Mail mail = new Mail();
            fileName = mail.addAttachment();
        }

        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                listView.Items.Clear();
                //listView.Items.Add(new Mail { Content = "Przykladowy tekst maila " + item });
            }
        }

        private void FetchMails_onClick(object sender, RoutedEventArgs e)
        {
            listView.Items.Clear();
            Mail mail = new Mail();
            List<Mail> listItems = mail.fetch();

            foreach (var item in listItems)
            {
                listView.Items.Add(item);
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView obj = sender as ListView;
            if (listView.Items.Count > 0)
            {
                Mail mail = listView.Items[obj.SelectedIndex] as Mail;
                mailContentBox.Text = mail.Content;
            }
        }

        private void E2checked(object sender, RoutedEventArgs e)
        {
            checkedEmployees[0] = !checkedEmployees[0];
        }

        private void E3checked(object sender, RoutedEventArgs e)
        {
            checkedEmployees[1] = !checkedEmployees[1];
        }

        private void E4checked(object sender, RoutedEventArgs e)
        {
            checkedEmployees[2] = !checkedEmployees[2];
        }


    }
}
