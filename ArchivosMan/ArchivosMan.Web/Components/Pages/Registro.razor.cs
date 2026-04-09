using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Registro
    {
        [Inject] private IUsuarioService UsuarioService { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Inject] private IWebHostEnvironment Env { get; set; } = default!;

        private RegistroModel modelo = new();
        private string MensajeError { get; set; } = string.Empty;
        private string MensajeOk { get; set; } = string.Empty;

        private const int RolDefaultId = Constantes.Usuario.RolDefault;
        public class RegistroModel
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "Correo no válido.")]
            public string Correo { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            [MinLength(6, ErrorMessage = "Mínimo 6 caracteres.")]
            public string Clave { get; set; } = string.Empty;

            [Required(ErrorMessage = "La confirmación es obligatoria.")]
            [Compare(nameof(Clave), ErrorMessage = "Las contraseñas no coinciden.")]
            public string ClaveConfirmacion { get; set; } = string.Empty;
        }
        private async Task OnSubmit()
        {
            MensajeError = string.Empty;
            MensajeOk = string.Empty;

            try
            {
                var usuario = new Usuario
                {
                    Nombre = modelo.Nombre,
                    Correo = modelo.Correo,
                    Clave = modelo.Clave, 
                    RolId = RolDefaultId
                };

                var creado = await UsuarioService.CrearAsync(usuario, Constantes.Usuario.Super);

                var rutaPlantilla = Path.Combine(
                    Env.WebRootPath, "templates", Constantes.Servicios.Template);

                if (File.Exists(rutaPlantilla))
                {
                    await UsuarioService.EnviarClave(creado.Correo, rutaPlantilla, 2);
                }

                MensajeOk = "Cuenta creada correctamente. Revisa tu correo para ver tu contraseña.";

                await Task.Delay(2000);
                Nav.NavigateTo("/login");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch (Exception ex)
            {
                MensajeError = "Error al crear la cuenta: " + ex.Message;
            }
        }
    }
}
