using API.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace API.Services.Notificaciones.Email;

public class EmailSender : IEmailSender
{
    private SmtpClient Cliente { get; }
    private EmailSenderOptions Options { get; }

    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<EmailSenderOptions> options, ILogger<EmailSender> logger)
    {
        _logger = logger;
        Options = options.Value;
        Cliente = new SmtpClient()
        {

            Host = Options.Host ?? "smtp.gmail.com",
            Port = Options.Port,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(Options.Email, Options.Password),
            EnableSsl = Options.EnableSsl
        };
    }

    public Task SendEmailAsync(string email, string subject, string message, byte[] adjunto)
    {
        var correo = new MailMessage(from: Options.Email ?? "", to: email,
                                        subject: subject, body: message);
        correo.IsBodyHtml = true;

        MemoryStream ms = new MemoryStream(adjunto);

        correo.Attachments.Add(new Attachment(ms, "ejemplo.pdf", "application/pdf"));

        try
        {
            return Cliente.SendMailAsync(correo);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR CORREO: " + ex);
            return Task.FromException(ex);
        }
    }
}
