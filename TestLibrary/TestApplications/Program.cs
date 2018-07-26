using System;
using System.Net.Mail;
using TestLibrary.Tools;

namespace TestApplications
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string contents = "Hello thre, <br>This is me";

            EmailManager email = new EmailManager("smtp.com", 25, "id", "password");
            email.From = "sender@test.com";
            email.To.Add("receiver@test.com");
            email.Subject = "Subject";
            email.Body = contents;
            email.Send();

            email.To.Clear();
            email.To.Add("receiver2@test.com");
            email.Subject = "Hi Derek";
            email.Send();

            /*
            EmailManager.Send("receiver@test.com", "Hi..", contents);
            EmailManager.Send("from@test.com", "receiver@test.com", "Hi..", contents);
            EmailManager.Send("from@test.com", "receiver@test.com", "Hi..", contents, "cc@test.com", "bcc@test.com");
            */

        }
    }
}
