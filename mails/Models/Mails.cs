namespace TodoApi.Models
{
    public class Mails
    {
        public long Id { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Recipients { get; set; }
        public DateTime DateMail { get; set; }
        public string? Result { get; set; }
        public string? FailedMessage { get; set; }
    }

}

