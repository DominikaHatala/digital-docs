using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.Xml;
using System.Text;


namespace digital_docs_wpf
{
    public class Mail
    {
        List<KeyValuePair<string, string>> employeeCredentails = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("dokumenty.cyfrowe2018@gmail.com", "DCDC2018"),
            new KeyValuePair<string, string>("dokumenty.cyfrowe2@gmail.com", "DCDC2018"),
            new KeyValuePair<string, string>("dokumenty.cyfrowe3@gmail.com", "DCDC2018"),
            new KeyValuePair<string, string>("dokumenty.cyfrowe4@gmail.com", "DCDC2018")
        };

        string mailClientAddress = "smtp.gmail.com";

        OpenFileDialog ofdAttachment;
        //public String fileName = "";

        public string ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        // public string Content { get; set; }
        public void send(int employeeNumber, string fileName, int employeesIncludedNumber)
        {
            string fromMail = employeeCredentails[0].Key;
            string fromPassword = employeeCredentails[0].Value;
            string toMail = employeeCredentails[employeeNumber - 1].Key;
            string mailSubject = "mailSubject";
            string mailBody = "mailBody";

            SmtpClient clientDetails = new SmtpClient();

            clientDetails.Port = 587;
            clientDetails.Host = mailClientAddress;
            clientDetails.EnableSsl = true;
            clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
            clientDetails.UseDefaultCredentials = false;
            clientDetails.Credentials = new NetworkCredential(fromMail, fromPassword);

            MailMessage mailDetails = new MailMessage();
            mailDetails.From = new MailAddress(fromMail);
            mailDetails.To.Add(toMail);
            mailDetails.Subject = mailSubject;
            mailDetails.Body = mailBody;

            mailDetails.Headers.Add("X-EmployeesIncludedNumber", employeesIncludedNumber.ToString());

            //file attachment
            if (fileName.Length > 0)
            {
                Attachment attachment = new Attachment(fileName);
                mailDetails.Attachments.Add(attachment);
            }

            clientDetails.Send(mailDetails);
            MessageBox.Show("Your mail has been sent.");
        }
        
        public void send(string fromMail, string toMail, string fileName)
        {
            string fromPassword = "DCDC2018";
            string mailSubject = "mailSubject";
            string mailBody = "mailBody";

            SmtpClient clientDetails = new SmtpClient();

            clientDetails.Port = 587;
            clientDetails.Host = mailClientAddress;
            clientDetails.EnableSsl = true;
            clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
            clientDetails.UseDefaultCredentials = false;
            clientDetails.Credentials = new NetworkCredential(fromMail, fromPassword);

            MailMessage mailDetails = new MailMessage();
            mailDetails.From = new MailAddress(fromMail);
            mailDetails.To.Add(toMail);
            mailDetails.Subject = mailSubject;
            mailDetails.Body = mailBody;

//            mailDetails.Headers.Add("X-EmployeesIncludedNumber", employeesIncludedNumber.ToString());

            //file attachment
            if (fileName.Length > 0)
            {
                Attachment attachment = new Attachment(fileName);
                mailDetails.Attachments.Add(attachment);
            }

            clientDetails.Send(mailDetails);
            MessageBox.Show("Your mail has been sent.");
        }

        public List<Mail> fetch(String currentUser)
        {
            List<Mail> listItems = new List<Mail>();
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
                if (string.Equals(currentUser, "user1"))
                {
                    objClient.Credentials = new NetworkCredential("dokumenty.cyfrowe2018@gmail.com", "DCDC2018");
                }
                else if (string.Equals(currentUser, "user2"))
                {
                    objClient.Credentials = new NetworkCredential("dokumenty.cyfrowe2@gmail.com", "DCDC2018");
                }
                else if (string.Equals(currentUser, "user3"))
                {
                    objClient.Credentials = new NetworkCredential("dokumenty.cyfrowe3@gmail.com", "DCDC2018");
                }
                else if (string.Equals(currentUser, "user4"))
                {
                    objClient.Credentials = new NetworkCredential("dokumenty.cyfrowe4@gmail.com", "DCDC2018");
                }

                //reading data and converting to string
                response = Encoding.UTF8.GetString(objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));

                response = response.Replace(@"<feed version=""0.3"" xmlns=""http://purl.org/atom/ns#"">", @"<feed>");

                //loading into an XML so we can get information easily
                doc.LoadXml(response);

                //nr of emails
                string nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;


                //Reading the title and the summary for every email
                foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))

                {
                    title = node.SelectSingleNode("title").InnerText;
                    content = node.SelectSingleNode("summary").InnerText;
                    id = node.SelectSingleNode("id").InnerText;
                    //Console.WriteLine(title);
                    //Console.WriteLine(summary);

                    //listView.Items.Clear();
                    Mail obj = new Mail {Title = title, Content = content};
                    listItems.Add(obj);
                }
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe);
                MessageBox.Show("Check your network connection");
            }

            return listItems;
        }

        public string addAttachment()
        {
            try
            {
                ofdAttachment = new OpenFileDialog();
                //ofdAttachment.Filter = "Images(.jpg,.png)|*.png;*.jpg;|Pdf Files|*.pdf";
                ofdAttachment.ShowDialog();
                string fileName = ofdAttachment.FileName;
                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return "";
        }
    }
}