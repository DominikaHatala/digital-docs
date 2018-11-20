using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for User234_InProgress.xaml
    /// </summary>
    public partial class User234_InProgress : Window
    {
        private String currentUser;
        public String fileName = "";
        public Mail selectedMail = null;

        public User234_InProgress(String currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void FetchMails_onClick(object sender, RoutedEventArgs e)
        {
            listView.Items.Clear();
            Mail mail = new Mail();
            List<Mail> listItems = mail.fetch(currentUser);

            foreach (var item in listItems)
            {
                listView.Items.Add(item);
            }
        }

        private void Accept_OnClick(object sender, RoutedEventArgs e)
        {
            Mail mail = new Mail();
            string fromMail = "dokumenty.cyfrowe2@gmail.com ";
            if (currentUser.Equals("user3"))
            {
                fromMail = "dokumenty.cyfrowe3@gmail.com";
            }
            else if (currentUser.Equals("user4"))
            {
                fromMail = "dokumenty.cyfrowe4@gmail.com";
            }
            mail.send(fromMail, "dokumenty.cyfrowe2018@gmail.com", fileName);
        }

        private void Attachment_OnClick(object sender, RoutedEventArgs e)
        {
            Mail mail = new Mail();
            fileName = mail.addAttachment();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView obj = sender as ListView;
            if (listView.Items.Count > 0)
            {
                Mail mail = listView.Items[obj.SelectedIndex] as Mail;
                selectedMail = mail;
                mailContentBox.Text = mail.Content;
            }
        }

        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new LoginView();
            newForm.Show();
            Close();
        }

        private void DownloadAttachment_OnClick(object sender, RoutedEventArgs e)
        {
            Mail mail = new Mail();
            //if (selectedMail != null && selectedMail.Attachment != null)
            //{
            string currentMail = "dokumenty.cyfrowe2@gmail.com ";
            if (currentUser.Equals("user3"))
            {
                currentMail = "dokumenty.cyfrowe3@gmail.com";
            }
            else if (currentUser.Equals("user4"))
            {
                currentMail = "dokumenty.cyfrowe4@gmail.com";
            }
            mail.downloadAttachment(currentMail, selectedMail.Content);
            //}
        }
    }
}