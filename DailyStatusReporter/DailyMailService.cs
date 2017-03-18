using OfficeOpenXml;
using System.Net.Mail;

namespace DailyStatusReporter
{
    public class DailyMailService
    {
        public void SendMail(string toAddress, string fromAddress, string subjectText, string bodyText, ExcelWorksheet excelWorkSheet)
        {
            try
            {
                
                MailMessage message = new MailMessage(fromAddress, toAddress, subjectText, bodyText);
                Attachment statusExcel = new Attachment(Properties.Settings.Default.FolderPath + "\\statusExcelSheetPath.xlsx");
                message.Attachments.Add(statusExcel);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("steppershofdsfdfdfdsffdfdtty@gmail.com", "");
                smtpClient.Send(message);
            }
            catch
            {
                throw;
            }


        }

        public void SendMailWithoutAttachment(string toAddress, string fromAddress, string subjectText, string bodyText)
        {
            try
            {

                MailMessage message = new MailMessage(fromAddress, toAddress, subjectText, bodyText);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("steppershotty@gmail.com", "Termin@t0r");
                smtpClient.Send(message);
            }
            catch
            {
                throw;
            }


        }
    }
}