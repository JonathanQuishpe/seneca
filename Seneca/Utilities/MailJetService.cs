using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Seneca.Models.Entities;
using System.Text;

namespace Seneca.Utilities
{
    public class MailJetService
    {
        private readonly MailjetSettings _settings;
        public MailJetService(IOptions<MailjetSettings> options)
        {
            _settings = options.Value;
        }

        // tipo 1 = validar correo, 2 recuperar contrasela
        public async Task SendMail(Usuario usuario, string subject, int tipo = 1)
        {
            try
            {
                string html = $@"
                            <h3>Hola {usuario.Nombres},</h3>
                            <p>Gracias por registrarte. Por favor, haz clic en el enlace a continuación para activar tu cuenta:</p>
                            <p><a href='{_settings.Url}/User/ValidateEmail/{usuario.Id}' target='_blank'>Da click aquí para activar tu cuenta</a></p>
                            <br />
                            <p>Saludos cordiales,</p>
                            <p>Seneca</p>
                     ";
                if(tipo == 2)
                {
                    html = $@"
                            <h3>Hola {usuario.Nombres},</h3>
                            <p>Tu contraseña temporal es :</p>
                            <p>{usuario.Contrasenia}</p>
                            <br />
                            <p>Saludos cordiales,</p>
                            <p>Seneca</p>
                     ";
                }
                MailjetClient client = new(_settings.ApiKey, _settings.ApiSecret);
                MailjetRequest request = new MailjetRequest {
                    Resource = Send.Resource,
                }.Property(Send.Messages, new JArray {
                 new JObject {
                     {"To",usuario.Nombres},
                     {"Subject", subject},
                     {"Text-part", subject},
                     {"Html-part", html},
                     {"CustomID","AppGettingStartedTest"},
                     {"FromEmail", "jonathanquishpecatagna@gmail.com"},
                     {"FromName", "Seneca"},
                     {
                         "Recipients",
                         new JArray {
                             new JObject {
                                 {"Email", usuario.CorreoElectronico},
                                 {"Name", usuario.Nombres}
                             }
                         }
                     }
                 }
                 });
                await client.PostAsync(request);

            }
            catch (Exception ex) {

                throw new InvalidOperationException(ex.Message);
            }
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new();
            Random random = new();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
