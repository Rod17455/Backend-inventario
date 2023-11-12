namespace API.Services.Notificaciones.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message, byte[] adjunto);


}
