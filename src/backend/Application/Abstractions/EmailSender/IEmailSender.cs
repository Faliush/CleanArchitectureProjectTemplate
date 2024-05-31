namespace Application.Abstractions.EmailSender;

public interface IEmailSender
{
    Task SendAsync(string email, string subject, string message);
}
