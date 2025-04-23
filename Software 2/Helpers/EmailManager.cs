using System.Net;
using System.Net.Mail;

namespace Software_2.Helpers
{
    public class EmailManager
    {
        private SmtpClient cliente;
        private MailMessage? email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "info.rescatesolidario@gmail.com"; 
        private string Password = "vinqajracetkbhgp"; 
        private bool EnabledSSL = true;

        public EmailManager()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }

        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtml = false)
        {
            try
            {
                email = new MailMessage(User, destinatario, asunto, mensaje);
                email.IsBodyHtml = esHtml;
                cliente.Send(email);
            }
            catch (SmtpException ex)
            {
                throw new Exception($"Error SMTP: {ex.StatusCode} - {ex.Message}");
            }
            finally
            {
                email?.Dispose();
            }
        }
    }
}