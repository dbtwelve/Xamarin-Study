using System.Net.Mail;
using System.Net;
using System.Configuration;
using System;

namespace TestLibrary.Tools
{
    public class EmailManager
    {
        private MailMessage _MailMessage;
        private SmtpClient _SmtpClient;

        public EmailManager(string host, int port, string id, string password)
        {
            _SmtpClient = new SmtpClient(host, port);
            _SmtpClient.Credentials = new NetworkCredential(id, password);

            _MailMessage = new MailMessage();
            _MailMessage.IsBodyHtml = true;
            _MailMessage.Priority = MailPriority.Normal;
        }

        public string From
        {
            get { return _MailMessage.From == null ? String.Empty : _MailMessage.From.Address; }
            set { _MailMessage.From = new MailAddress(value); }
            
        }

        public MailAddressCollection To
        {
            get { return _MailMessage.To; }
        }

        public string Subject
        {
            get { return _MailMessage.Subject; }
            set { _MailMessage.Subject = value; }
        }

        public string Body
        {
            get { return _MailMessage.Body; }
            set { _MailMessage.Body = value; }
        }

        public void Send()
        {
            _SmtpClient.Send(_MailMessage);
        }



#region Static Methods
        public static void Send(string from, string to, string subject, string contents, string cc, string bcc)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("Sender is empty.");
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("To is empty.");
            
            //string sender = ConfigurationManager.AppSettings["SMTPSender"];// "do_not_reply@test.com";

            string smtpHost = ConfigurationManager.AppSettings["SMTPHost"];//"smtp.com";
            int smtpPort = 0;

            if (ConfigurationManager.AppSettings["SMTPPort"] == null ||
                int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out smtpPort) == false)
                smtpPort = 25;

            string smtpId = "id";
            string smtpPwd = "password";



            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(from);
            mailMsg.To.Add(to);

            if (!string.IsNullOrEmpty(cc))
                mailMsg.CC.Add(cc);
            if (!string.IsNullOrEmpty(bcc))
                mailMsg.Bcc.Add(bcc);


            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = contents;
            mailMsg.Priority = MailPriority.Normal;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(smtpId, smtpPwd);
            smtpClient.Host = smtpHost;
            smtpClient.Port = smtpPort;

            smtpClient.Send(mailMsg);

        }

        public static void Send(string from, string to, string subject, string contents)
        { 
            Send(from, to, subject, contents, null, null);
        }

        public static void Send(string to, string subject, string contents)
        {

            string sender = ConfigurationManager.AppSettings["SMTPSender"];// "do_not_reply@test.com";

            Send(sender, to, subject, contents);

        }
#endregion


    }
}
