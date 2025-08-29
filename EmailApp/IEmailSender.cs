public interface IEmailSender
{
    void Send(MailItem item);
}

public class EmailSender : IEmailSender
{
    public void Send(MailItem item)
    {
        Console.WriteLine($"Email Sent To:  {item.Address}");
        Console.WriteLine($"Body: {item.Body}");
    }
}