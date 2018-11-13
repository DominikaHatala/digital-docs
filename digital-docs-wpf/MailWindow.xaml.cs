using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace digital_docs_wpf
{
    public partial class MailWindow : Window
    {
        public MailWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new AssignEmployeeWindow();
            newForm.Show();
            Close();
        }

        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                listView.Items.Clear();
                listView.Items.Add(new Mail { Content = "Przykladowy tekst maila " + item });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Net.WebClient objClient = new System.Net.WebClient();
                string response;
                string title;
                string summary;

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
                    summary = node.SelectSingleNode("summary").InnerText;
                    Console.WriteLine(title);
                    Console.WriteLine(summary);

                    //listView.Items.Clear();
                    listView.Items.Add(new Mail { Content = title + '\n' + summary + "\n\n" });
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show("Check your network connection");
            }
        }
    }
}
