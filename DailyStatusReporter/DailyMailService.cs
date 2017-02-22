using System.Net.Mail;

namespace DailyStatusReporter
{
    public class DailyMailService
    {
        public void SendMail(string toAddress, string fromAddress, string subjectText, string bodyText)
        {
            try
            {
                MailMessage message = new MailMessage(fromAddress, toAddress, subjectText, bodyText);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("steppershotty@gmail.com", "");
                smtpClient.Send(message);
            }
            catch
            {
                throw;
            }


        }
    }
}