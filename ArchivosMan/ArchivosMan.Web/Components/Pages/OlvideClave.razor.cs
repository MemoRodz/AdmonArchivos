using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class OlvideClave
    {
        [Inject] private IUsuarioService UsuarioService { get; set; } = default!;
        [Inject] private IWebHostEnvironment Env { get; set; } = default!;

        protected OlvideClaveModel modelo = new();
        protected string MensajeError { get; set; } = string.Empty;
        protected string MensajeOk { get; set; } = string.Empty;
        protected bool Procesando { get; set; } = false;

        public class OlvideClaveModel
        {
            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "Correo no válido.")]
            public string Correo { get; set; } = string.Empty;
        }

        protected async Task OnSubmit()
        {
            MensajeError = string.Empty;
            MensajeOk = string.Empty;
            Procesando = true;

            try
            {
                var rutaPlantilla = Path.Combine(
                    Env.WebRootPath, "templates", Constantes.Servicios.Template);

                var ok = await UsuarioService.RestablecerClave(
                    modelo.Correo,
                    rutaPlantilla);

                if (!ok)
                {
                    MensajeError = "No fue posible restablecer la contraseña. Intente más tarde.";
                    return;
                }

                MensajeOk = "Si el correo existe en el sistema, se envió una nueva contraseña.";
                modelo = new OlvideClaveModel(); 
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al restablecer la contraseña: {ex.Message}";
            }
            finally
            {
                Procesando = false;
            }
        }
    }
}
