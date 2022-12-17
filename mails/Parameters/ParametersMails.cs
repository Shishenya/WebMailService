namespace TodoApi.Models
{
    public static class ParametersMails
    {
        private static string userMail = "mail@mail.com";
        private static string userPassword = "passwordMail";
        private static string stmpClient = "mySmtp.smtp.ru";
        private static int smptPort = 123;

        public static string UserMail { get => userMail; }
        public static string UserPassword { get => userPassword; }
        public static string StmpClient { get => stmpClient; }
        public static int SmtpPort { get => smptPort; }

    }
}
