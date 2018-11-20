using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace digital_docs_wpf
{
    public partial class User1_ToComplete : Window
    {
        bool[] checkedEmployees = new bool[] { false, false, false }; // employees: 2,3,4

        public String fileName = "";

        public User1_ToComplete()
        {
            InitializeComponent();
        }

        private void Accept_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < checkedEmployees.Length; i++)
            {
                if (checkedEmployees[i])
                {
                    Mail mail = new Mail();
                    mail.send(i + 2, fileName, checkedEmployees.Length);
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
            List<Mail> listItems = mail.fetch("user1");

           
            Dictionary<string, KeyValuePair<int, int>> sync 
                = new Dictionary<string, KeyValuePair<int, int>>();

            foreach (var item in listItems)
            {
                listView.Items.Add(item);

                if(sync.Count == 0 || !sync.ContainsKey(item.ID)) //zliczaj maile kt�re przysz�y
                    sync.Add(item.ID, new KeyValuePair<int, int>(/*employeesIncluded*/3, 0));

                sync[item.ID] = new KeyValuePair<int, int>(sync[item.ID].Key,sync[item.ID].Value+1);
            }

            foreach (var v in sync) {
                if (v.Value.Key == v.Value.Value) { //sprawdzaj ilo�� maili
                    List<Mail> mails = new List<Mail>(); //kontener na maile kt�re chcemy zebra�
                    foreach (var m in listItems) 
                        if (m.ID == v.Key)
                            mails.Add(m);
                    
                    /*tutaj scalanie xmli, zgubi�em si� troch� w projekcie*/
                }
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


        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new LoginView();
            newForm.Show();
            Close();
        }

        private void OpenNewTasks_OnClick(object sender, RoutedEventArgs e) {
            var newForm = new User1_NewTasks();
            newForm.Show();
            Close();
        }
    }
}
