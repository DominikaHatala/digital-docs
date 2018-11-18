using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Windows;

namespace digital_docs_wpf
{
    public class Mail
    {
        List<KeyValuePair<string, string>> employeeCredentails = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("dokumenty.cyfrowe2018@gmail.com", "DCDC2018"),
            new KeyValuePair<string, string>("e2mail", "e2password"),
            new KeyValuePair<string, string>("e3mail", "e3password"),
            new KeyValuePair<string, string>("e4mail", "e4password"),
        };

        string mailClientAddress = "smtp.gmail.com";

        public string ID { get; set; }

        public string Title { get; set; }

       // public string Content { get; set; }
       public void send(int employeeNumber)
        {            
            string fromMail = employeeCredentails[0].Key;
            string fromPassword = employeeCredentails[0].Value;
            string toMail = employeeCredentails[employeeNumber - 1].Key;
            string mailSubject = "mailSubject";
            string mailBody = "mailBody";

            SmtpClient clientDetails = new SmtpClient();

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

            // @todo: decide what have to be send...

            //mailDetails.Headers.Add("X-SampleHeader", "Just works!");

            // @todo: attach file

            clientDetails.Send(mailDetails);
            MessageBox.Show("Your mail has been sent.");
       
        }

        public void fetch()
        {

        }

    }

    
}