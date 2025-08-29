public interface IEmailService
{
    void SendEmail(string body, string address);
}

public class EmailService : IEmailService
{
    private readonly IEmailSender _sender;
    public EmailService(IEmailSender sender)
    {
        _sender = sender;
    }
    public void SendEmail(string body, string address)
    {
        MailItem item = new MailItem()
        {
            Body = body,
            Address = address
        };
        _sender.Send(item);
    }
}