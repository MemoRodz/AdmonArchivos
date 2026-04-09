using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Contactos
    {
        [Inject] private IContactoService ContactoService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;

        protected List<Contacto> ListaContactos { get; set; } = new();
        protected Contacto? ContactoEditando { get; set; }

        protected bool IsLoading { get; set; }
        protected bool ModoEdicion { get; set; }

        #region Mensajes
        protected string MensajeError { get; set; } = "";
        protected string MensajeOk { get; set; } = "";
        #endregion

        #region Flag por Rol
        protected bool PuedeCrear => Auth.EsSuper || Auth.EsAdmin || Auth.EsEmpleado || Auth.EsSupervisor;
        protected bool PuedeEditar => Auth.EsSuper || Auth.EsAdmin || Auth.EsSupervisor;
        protected bool PuedeEliminar => Auth.EsSuper || Auth.EsAdmin;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await CargarContactos();
        }

        private async Task CargarContactos()
        {
            IsLoading = true;
            try
            {
                MensajeError = string.Empty;
                MensajeOk = string.Empty;
                ListaContactos = await ContactoService.ListarAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }
        protected void NuevoContacto()
        {
            try
            {
                var nuevo = new Contacto
                {
                    IdContacto = 0,
                    Nombre = "",
                    Correo = "",
                    Telefono = ""
                };
                ListaContactos?.Insert(0, nuevo);
                ContactoEditando = nuevo;
                ModoEdicion = true;
                MensajeOk = $"Nuevo Contacto '{nuevo.Nombre}' creado.";
            }
            catch (Exception)
            {
                MensajeError = $"Error al crear Contacto.";
            }
        }
        protected void EditarContacto(Contacto contacto)
        {
            ContactoEditando = contacto;
            ModoEdicion = true;
        }
        protected void CancelarEdicion()
        {
            ContactoEditando = null;
            ModoEdicion = false;
        }
        protected async Task GuardarEdicion()
        {
            if (ContactoEditando == null) return;
            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            if (ContactoEditando.IdContacto == 0)
            {
                await ContactoService.CrearAsync(ContactoEditando, usuarioId);
            }
            else
            {
                await ContactoService.ActualizarAsync(ContactoEditando, usuarioId);
            }
            ContactoEditando = null;
            ModoEdicion = false;
            await CargarContactos();
        }

        protected async Task EliminarContacto(int id)
        {
            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            await ContactoService.EliminarAsync(id, usuarioId);
            await CargarContactos();
        }
    }
}
