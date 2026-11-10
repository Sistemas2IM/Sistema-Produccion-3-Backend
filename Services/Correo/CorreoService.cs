using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Sistema_Produccion_3_Backend.Services.Correo
{
    public class SmtpOptions
    {
        public const string Seccion = "Smtp";

        public string Host { get; set; } = "smtp.gmail.com";
        public int Puerto { get; set; } = 587;
        public bool UsarSsl { get; set; } = true;
        public string Usuario { get; set; } = "";
        // Contraseña de aplicación de Google: configurarla en user-secrets o variable de entorno (Smtp__Password), no en el repo
        public string Password { get; set; } = "";
        public string Remitente { get; set; } = "";
        public string NombreRemitente { get; set; } = "Notificaciones SAP";
    }

    public interface ICorreoService
    {
        Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml, CancellationToken ct);
    }

    // Envío por SMTP de Google Workspace
    public class SmtpCorreoService : ICorreoService
    {
        private readonly IOptionsMonitor<SmtpOptions> _options;

        public SmtpCorreoService(IOptionsMonitor<SmtpOptions> options)
        {
            _options = options;
        }

        public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml, CancellationToken ct)
        {
            var opt = _options.CurrentValue;
            var remitente = string.IsNullOrWhiteSpace(opt.Remitente) ? opt.Usuario : opt.Remitente;

            using var mensaje = new MailMessage
            {
                From = new MailAddress(remitente, opt.NombreRemitente),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            mensaje.To.Add(destinatario);

            // SmtpClient no es thread-safe: uno por envío
            using var cliente = new SmtpClient(opt.Host, opt.Puerto)
            {
                EnableSsl = opt.UsarSsl,
                Credentials = new NetworkCredential(opt.Usuario, opt.Password)
            };

            await cliente.SendMailAsync(mensaje, ct);
        }
    }
}
