using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

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

    // Archivo adjunto ya leído en memoria
    public record ArchivoCorreo(string Nombre, string TipoMime, byte[] Contenido);

    public interface ICorreoService
    {
        Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml, CancellationToken ct,
            IReadOnlyCollection<ArchivoCorreo>? adjuntos = null);
    }

    // Envío por SMTP de Google Workspace
    public class SmtpCorreoService : ICorreoService
    {
        private readonly IOptionsMonitor<SmtpOptions> _options;

        public SmtpCorreoService(IOptionsMonitor<SmtpOptions> options)
        {
            _options = options;
        }

        public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml, CancellationToken ct,
            IReadOnlyCollection<ArchivoCorreo>? adjuntos = null)
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

            // Un stream propio por correo: los bytes se pueden reutilizar en varios envíos.
            // Al liberar el MailMessage se liberan también los adjuntos
            foreach (var archivo in adjuntos ?? Array.Empty<ArchivoCorreo>())
            {
                mensaje.Attachments.Add(new Attachment(new MemoryStream(archivo.Contenido, writable: false), archivo.Nombre, archivo.TipoMime)
                {
                    NameEncoding = Encoding.UTF8
                });
            }

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
