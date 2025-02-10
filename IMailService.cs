namespace asp2.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}