using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Proyectos
    {
        [Inject] private IProyectoService ProyectoService { get; set; } = default!;
        [Inject] private IArchivoService ArchivoService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;

        protected List<Proyecto> ListaProyectos { get; set; } = new();
        protected List<Archivo> ListaArchivos { get; set; } = new();
        protected Proyecto? ProyectoEditando { get; set; }
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
            await CargarProyectos();
        }
        private async Task CargarProyectos()
        {
            IsLoading = true;
            try
            {
                MensajeError = "";
                MensajeOk = "";
                ListaProyectos = await ProyectoService.ListarAsync();
                ListaArchivos = await ArchivoService.ListarAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void NuevoProyecto()
        {
            try
            {
                var nuevo = new Proyecto
                {
                    IdProyecto = 0,
                    Nombre = "",
                    IconoId = ListaArchivos.FirstOrDefault()?.IdArchivo ?? 0
                };
                ListaProyectos?.Insert(0, nuevo);
                ProyectoEditando = nuevo;
                ModoEdicion = true;
                MensajeOk = $"Nuevo Proyecto '{nuevo.Nombre}' creado.";
            }
            catch (Exception)
            {
                MensajeError = $"Error al crear Proyecto.";
            }
        }

        protected void EditarProyecto(Proyecto proyecto)
        {
            ProyectoEditando = proyecto;
            ModoEdicion = true;
        }

        protected void CancelarEdicion()
        {
            ProyectoEditando = null;
            ModoEdicion = false;
        }

        protected async Task GuardarEdicion()
        {
            if (ProyectoEditando == null) return;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;

            if (ProyectoEditando.IdProyecto == 0)
            {
                await ProyectoService.CrearAsync(ProyectoEditando, usuarioId);
            }
            else
            {
                await ProyectoService.ActualizarAsync(ProyectoEditando, usuarioId);
            }
            ProyectoEditando = null;
            ModoEdicion = false;
            await CargarProyectos();
        }

        protected async Task EliminarProyecto(int id)
        {
            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            await ProyectoService.EliminarAsync(id, usuarioId);
            await CargarProyectos();
        }

        protected string ObtenerTextoArchivo(Archivo archivo)
        {
            if (archivo == null)
                return "Archivo";

            if (!string.IsNullOrWhiteSpace(archivo.Nombre))
                return archivo.Nombre;

            if (!string.IsNullOrWhiteSpace(archivo.Url))
                return archivo.Url;

            return $"Archivo #{archivo.IdArchivo}";
        }

        protected string ObtenerUrlIcono(int? iconoId)
        {
            if (!iconoId.HasValue || iconoId.Value <= 0)
                return "/images/no-image.png"; // o string.Empty si no tienes imagen por defecto

            return ListaArchivos.FirstOrDefault(a => a.IdArchivo == iconoId.Value)?.Url
                   ?? "/images/no-image.png";
        }
    }
}
