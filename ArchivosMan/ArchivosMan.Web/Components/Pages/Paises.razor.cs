using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Paises
    {
        [Inject] private IPaisService PaisService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;

        protected List<Pais> ListaPaises { get; set; } = new();
        protected Pais? PaisEditando { get; set; }
        protected bool IsLoading { get; set; }
        protected bool ModoEdicion { get; set; }

        #region Mensajes en Pantalla
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
            await CargarPaises();
        }

        private async Task CargarPaises()
        {
            IsLoading = true;
            try
            {
                ListaPaises = await PaisService.ListarAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void NuevoPais()
        {
            try
            {
                var nuevo = new Pais { IdPais = 0, Nombre = "" };
                ListaPaises.Insert(0, nuevo);
                PaisEditando = nuevo;
                ModoEdicion = true;
                MensajeOk = $"Nuevo País '{nuevo.Nombre}' creado.";
            }
            catch (Exception)
            {
                MensajeError = $"Error al crear País.";
            }
        }

        protected void EditarPais(Pais pais)
        {
            PaisEditando = pais;
            ModoEdicion = true;
        }

        protected void CancelarEdicion()
        {
            PaisEditando = null;
            ModoEdicion = false;
        }

        protected async Task GuardarEdicion()
        {
            if (PaisEditando == null) return;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;

            if (PaisEditando.IdPais == 0)
            {
                await PaisService.CrearAsync(PaisEditando, usuarioId);
            }
            else
            {
                await PaisService.ActualizarAsync(PaisEditando, usuarioId);
            }

            PaisEditando = null;
            ModoEdicion = false;
            await CargarPaises();
        }

        protected async Task EliminarPais(int id)
        {
            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            await PaisService.EliminarAsync(id, usuarioId);
            await CargarPaises();
        }
    }
}
