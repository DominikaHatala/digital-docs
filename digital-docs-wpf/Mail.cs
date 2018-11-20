using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.Xml;
using System.Text;
using OpenPop.Pop3;
using System.IO;
using OpenPop.Mime;

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
        Random rnd = new Random();


        public string ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Attachment { get; set; }

        // public string Content { get; set; }
        public void send(int employeeNumber, string fileName, int employeesIncludedNumber, string orderId = "")
        {
            string fromMail = employeeCredentails[0].Key;
            string fromPassword = employeeCredentails[0].Value;
            string toMail = employeeCredentails[employeeNumber - 1].Key;
            int random = rnd.Next(1111, 9999);
            string mailSubject = "mailSubject" + random.ToString();
            string mailBody = "mailBody" + random.ToString();

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
            mailDetails.Headers.Add("X-OrderId", orderId);
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
            int random = rnd.Next(1111, 9999);
            string fromPassword = "DCDC2018";
            string mailSubject = "mailSubject" + random.ToString(); ;
            string mailBody = "mailBody" + random.ToString();

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
                string attachment;

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
                    //attachment = node.SelectSingleNode("attachment").InnerText;

                    //Console.WriteLine(title);
                    //Console.WriteLine(summary);

                    //listView.Items.Clear();
                    Mail obj = new Mail {Title = title, Content = content, ID = id /*, Attachment = attachment */ };
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

        public void downloadAttachment(String mail, String selectedMailContent)
        {
            String mailContent = selectedMailContent;
            var client = new Pop3Client();
            try
            {
                System.IO.Directory.CreateDirectory(@"C:\Attachment");
                client.Connect("pop.gmail.com", 995, true); //UseSSL true or false
                client.Authenticate(mail, "DCDC2018");

                var messageCount = client.GetMessageCount();
                var Messages = new List<Message>(messageCount);

                Console.WriteLine("nieodczytanych wiadomosci: " + messageCount);

                for (int i = 0; i < messageCount; i++)
                {
                    Message getMessage = client.GetMessage(i + 1);
                    Messages.Add(getMessage);
                }

                foreach (Message msg in Messages)
                {
                    StringBuilder builder = new StringBuilder();
                    MessagePart plainText = msg.FindFirstPlainTextVersion();
                    builder.Append(plainText.GetBodyAsText());
                    //MessageBox.Show(builder.ToString());
                    string s = builder.ToString();
                    string sFromMail = s.Substring(0, 12);
                    string sFromSelect = selectedMailContent.Substring(0, 12);

                    //if (sFromMail.Equals(sFromSelect))
                    //{
                        foreach (var attachment in msg.FindAllAttachments())
                        {
                            string filePath = Path.Combine(@"C:\Attachment", attachment.FileName);

                            FileStream Stream = new FileStream(filePath, FileMode.Create);
                            BinaryWriter BinaryStream = new BinaryWriter(Stream);
                            BinaryStream.Write(attachment.Body);
                            BinaryStream.Close();
                            Console.WriteLine("zapisano zalacznik: " + attachment.FileName);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("", ex.Message);
            }
            finally
            {
                if (client.Connected)
                    client.Dispose();
            }
        }
    }
}