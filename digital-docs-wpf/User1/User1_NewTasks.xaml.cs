using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Net.Mail;
using System.Net;
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
            for (int i = 0 ; i < checkedEmployees.Length; i++)
            {
                if (checkedEmployees[i])
                {
                    Mail mail = new Mail();
                    mail.send(i+2, fileName);
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
            try
            {
                System.Net.WebClient objClient = new System.Net.WebClient();
                string id;
                string response;
                string title;
                string content;

                //Creating a new xml document
                XmlDocument doc = new XmlDocument();

                //Logging in Gmail server to get data
                objClient.Credentials = new System.Net.NetworkCredential("dokumenty.cyfrowe2018@gmail.com", "DCDC2018");
                //reading data and converting to string
                response = Encoding.UTF8.GetString(objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));

                response = response.Replace(@"<feed version=""0.3"" xmlns=""http://purl.org/atom/ns#"">", @"<feed>");

                //loading into an XML so we can get information easily
                doc.LoadXml(response);

                //nr of emails
                string nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;

                listView.Items.Clear();
                //Reading the title and the summary for every email
                foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))

         
                {
                    title = node.SelectSingleNode("title").InnerText;
                    content = node.SelectSingleNode("summary").InnerText;
                    id = node.SelectSingleNode("id").InnerText;
                    //Console.WriteLine(title);
                    //Console.WriteLine(summary);

                    //listView.Items.Clear();
                    Mail obj = new Mail { Title = title };
                    listView.Items.Add(obj);
                }
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe);
                MessageBox.Show("Check your network connection");
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView obj = sender as ListView;
            Console.WriteLine(obj.SelectedItem);
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
