using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ArchivosMan.BLL.Services;
using ArchivosMan.BLL.Interfaces;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Login
    {
        [Inject] private IUsuarioService UsuarioService { get; set; } = default!;
        [Inject] private AuthState AuthState { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        protected string Correo { get; set; } = "";
        protected string Clave { get; set; } = "";
        protected string MensajeError { get; set; } = "";
        protected bool MostrarClave { get; set; } = false;

        protected async Task IniciarSesion()
        {
            MensajeError = string.Empty;
            var usuario = await UsuarioService.ValidarCredencialesAsync(Correo, Clave);

            if (usuario == null)
            {
                MensajeError = "Correo o clave incorrectos.";
                return;
            }

            AuthState.EstablecerUsuario(usuario);
            // Redirección según rol
            if (AuthState.EsSuper || AuthState.EsAdmin)
            {
                Nav.NavigateTo("/administracion/usuarios");
            }
            else
            {
                // Los demás perfiles se van a Inicio
                Nav.NavigateTo("/");
            }
        }

        private void OnKeyDownHandlerLogin(KeyboardEventArgs e)
        {
            if (!string.IsNullOrEmpty(Correo) || !string.IsNullOrEmpty(Clave))
            {
                if (e.Key == "Enter")
                {
                    _ = IniciarSesion();
                }
            }
            else
            {
                MensajeError = "Correo o Clave vacias.";
            }

        }

        protected void AlternarMostrarClave()
        {
            MostrarClave = !MostrarClave;
        }
    }
}
