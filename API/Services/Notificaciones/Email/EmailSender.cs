using API.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
        //string htmlContent = PlantillaEjemplo();
        string pdfFileName = "output.pdf";

        try
        {
            // Convert HTML content to PDF
           /* using (FileStream stream = new FileStream(pdfFileName, FileMode.Create))
            {
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();
                    using (StringReader reader = new StringReader(message))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);
                    }
                    document.Close();
                }
            }*/

            var correo = new MailMessage(from: Options.Email ?? "", to: email,
                                        subject: subject, body: message);
            correo.IsBodyHtml = true;

            MemoryStream ms = new MemoryStream(adjunto);
            Attachment attachment = new Attachment(ms, "Ejemplo.pdf", "application/pdf");
            correo.Attachments.Add(attachment);
            _logger.LogInformation("Correo enviado");
            return Cliente.SendMailAsync(correo);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR CORREO: " + ex);
            return Task.FromException(ex);
        }
    }

    private string PlantillaEjemplo()
    {
        string _plantilla = string.Empty;
        //System.IO.File.ReadAllText
        using (StreamReader reader = new StreamReader("Recursos\\adjunto.html"))
        {
            _plantilla = reader.ReadToEnd();
        }

        return _plantilla;
    }
}
