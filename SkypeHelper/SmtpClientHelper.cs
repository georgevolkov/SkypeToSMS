using System.Net.Mail;

namespace SkypeHelper
{
    public static class SmtpClientHelper
    {
        public static SmtpClient SetCredentails(SkypeConfiguration configuration)
        {

            var mail = GetMailAdress(configuration);

            var client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.mail.ru",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(mail, "qwerty")
            };
            return client;
        }

        public static string GetMailAdress(SkypeConfiguration configuration)
        {
            var expiredTimes = configuration.ExpiredTimes != 0
               ? configuration.ExpiredTimes.ToString() : string.Empty;
            var mail = $"skypetosms{expiredTimes}@mail.ru";
            return mail;
        }
    }
}
