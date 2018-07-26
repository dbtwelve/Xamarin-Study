using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace TestLibrary.Tools
{
    public class EmailManager
    {
        public static void Send(string to, string subject, string contents)
        {

            string sender = ConfigurationManager.AppSettings["SMTPSender"];// "do_not_reply@test.com";

            string smtpHost = ConfigurationManager.AppSettings["SMTPHost"];//"smtp.com";
            int smtpPort = 0;
            
            if (ConfigurationManager.AppSettings["SMTPPort"] == null || 
                int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out smtpPort) == false)
                smtpPort = 25;
                                    
            string smtpId = "id";
            string smtpPwd = "password";



            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(sender);
            mailMsg.To.Add(to);

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

        public EmailManager()
        {
        }
    }
}
