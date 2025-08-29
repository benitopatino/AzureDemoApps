namespace EmailApp;

class Program
{
    static void Main(string[] args)
    {
        IEmailService service = new EmailService(new EmailSender());
        service.SendEmail("This is the best way to do something.", "oneemail@test.com");
        service.SendEmail("This is the best way to do something.", "twoemail@test.com");

    }
}
