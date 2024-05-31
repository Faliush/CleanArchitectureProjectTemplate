using System.Net;
using System.Net.Mail;

namespace Application.Abstractions.EmailSender;

internal sealed class EmailSender : IEmailSender
{
    private const string _mail = "email@gmail.com";
    private const string _password = "password";

    public Task SendAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_mail, _password),
            EnableSsl = true
        };

        var body = GetHtmlBody(message);

        var mailMessage = new MailMessage(
            from: _mail,
            to: email,
            subject: subject,
            body: body);

        return smtpClient.SendMailAsync(mailMessage);
    }

    private static string GetHtmlBody(string message)
    {
        string htmlContent = @"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
                .container { max-width: 600px; margin: 50px auto; padding: 20px; background-color: #ffffff; }
                .header { background-color: #4CAF50; color: white; padding: 10px; text-align: center; }
                .content { padding: 20px; text-align: center; }
                .footer { background-color: #ddd; padding: 10px; text-align: center; margin-top: 20px; }
                .button { background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; }
                .regards { margin-top: 60px; }
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Welcome to Our Community!</h1>
                    </div>
                    <div class='content'>
                        <p>Hi there,</p>
                        <p>"+ message +@"<p>
                        <p>If you have any questions, feel free to reach out to our support team.</p>
                        <p class='regards'>Best regards,<br>Your Company Name</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2024 Your Company Name. All rights reserved.</p>
                    </div>
                </div>
            </body>
        </html>";

        return htmlContent;
    }
}
