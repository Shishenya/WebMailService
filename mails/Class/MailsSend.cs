using System.Net;
using System.Net.Mail;

namespace TodoApi.Models
{
    public class MailsSend
    {
        private string userMail;
        private string userPassword;
        private string smtpClient;
        private int smtpPort;

        public MailsSend(string userMail, string userPassword, string smtpClient, int smtpPort)
        {
            this.userMail = userMail;
            this.userPassword = userPassword;
            this.smtpClient = smtpClient;
            this.smtpPort = smtpPort;           

        }

        public (string,string) SendMyEmail(string subject, string body, string toEmail)
        {

            string result = "OK";
            string failedMessage = "";

            try
            {

                MailAddress from = new MailAddress(userMail);
                MailAddress to = new MailAddress(toEmail);
                MailMessage message = new MailMessage(from, to) // Формируем сообщение с нужными заголовками
                                                                // Заголовок "от кого" ныне часто игнорируется,
                                                                // и на его место ставится реальный адрес отправителя,
                                                                // так что подписаться миллионером дядей Петей не получится
                {
                    Subject = subject,
                    IsBodyHtml = false,
                    Body = body
                };

                SmtpClient smtp = new SmtpClient(smtpClient, smtpPort)
                {
                    Credentials = new NetworkCredential(userMail, userPassword),
                    EnableSsl = true
                };

                smtp.Send(message); 

            }
            catch (FormatException)
            {
                result = "FAILED";
                failedMessage = "Wrong E-mail format";
            }
            catch (ArgumentException)
            {
                result = "FAILED";
                failedMessage = "Wrong argument";
            }
            catch (Exception ex)
            {
                result = "FAILED";
                failedMessage = ex.ToString();
            }

            return (result,failedMessage);
        }

    }
}
