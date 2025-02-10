namespace asp2.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "oluwadarasimifajolu@gmail.com";
        private string _mailFrom = "noreply@aycompany.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailFrom}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }

    }
}
