using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Contactame
    {
        [Inject] private ICorreoService CorreoService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;
        private const string CorreoDestino = Constantes.Servicios.CorreoContacto;

        protected string Correo { get; set; } = string.Empty;
        protected string Mensaje { get; set; } = string.Empty;
        protected string MensajeError { get; set; } = string.Empty;
        protected string MensajeOk { get; set; } = string.Empty;
        protected bool Procesando { get; set; } = false;

        protected override void OnInitialized()
        {
            if (Auth.EstaAutenticado && Auth.UsuarioActual is not null)
            {
                Correo = Auth.UsuarioActual.Correo;
            }
        }

        protected async Task EnviarMensaje()
        {
            MensajeError = string.Empty;
            MensajeOk = string.Empty;

            if (string.IsNullOrWhiteSpace(Correo))
            {
                MensajeError = "El correo es obligatorio.";
                return;
            }

            // Validar estructura del correo
            if (!System.Text.RegularExpressions.Regex.IsMatch(Correo,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MensajeError = "El formato del correo no es válido.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Mensaje))
            {
                MensajeError = "El mensaje es obligatorio.";
                return;
            }

            if (Mensaje.Length > 1000)
            {
                MensajeError = "El mensaje no puede superar 1,000 caracteres.";
                return;
            }

            Procesando = true;

            try
            {
                string mensajeHtml = Mensaje.Replace(Environment.NewLine, "<br/>");
                // Correo 1: Destinatario.
                string cuerpoDestino = $@"
                    <div style='font-family:Arial,sans-serif;'>
                        <h3>Nuevo mensaje de contacto</h3>
                        <p><strong>De:</strong> {Correo}</p>
                        <hr/>
                        <p>{mensajeHtml}</p>
                    </div>";

                var okDestino = await CorreoService.EnviarCorreo(
                    CorreoDestino,
                    $"Contacto desde la app - {Correo}",
                    cuerpoDestino);

                if (!okDestino)
                {
                    MensajeError = "No fue posible enviar el mensaje. Intente más tarde.";
                    return;
                }

                // Correo 2: Confirmación al remitente.
                string cuerpoConfirmacion = $@"
                    <div style='font-family:Arial,sans-serif;background-color:#EDF6FF;padding:20px;'>
                        <div style='max-width:500px;margin:auto;background:#fff;
                                    border-radius:5px;padding:20px;
                                    box-shadow:0px 0px 10px #DEDEDE;'>
                            <h3 style='color:#004DAF;'>Confirmación de contacto</h3>
                            <p>Gracias por contactarnos {Correo}.>
                            <p>Hemos recibido tu mensaje. Nos pondremos en contacto contigo a la brevedad.</p>
                            <p><strong>Este fue el mensaje que nos enviaste:</strong></p>
                            <hr/>
                            <p>{mensajeHtml}</p>
                            <hr/>
                            <div style='background-color:#FFE1CE;padding:15px;margin-top:15px;border-radius:4px;'>
                                <p style='margin:0;color:#F45E00;'>
                                    Si no realizaste este contacto, puedes ignorar este correo.
                                </p>
                            </div>
                        </div>
                    </div>";

                // Enviamos confirmación al remitente; si falla no bloqueamos el flujo principal
                await CorreoService.EnviarCorreo(
                    Correo,
                    "Confirmación de contacto - Recibimos tu mensaje",
                    cuerpoConfirmacion);
                MensajeOk = "Mensaje enviado correctamente. Nos pondremos en contacto pronto.";
                Limpiar();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al enviar el mensaje: {ex.Message}";
            }
            finally
            {
                Procesando = false;
            }
        }

        protected void Limpiar()
        {
            Correo = Auth.EstaAutenticado && Auth.UsuarioActual is not null
                ? Auth.UsuarioActual.Correo
                : string.Empty;
            Mensaje = string.Empty;
            MensajeError = string.Empty;
            MensajeOk = string.Empty;
        }
    }
}
