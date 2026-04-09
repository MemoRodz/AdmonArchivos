using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;
using ArchivosMan.Web.Models;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Archivos
    {
        [Inject] private IArchivoService ArchivoService { get; set; } = default!;
        [Inject] private ICategoriaService CategoriaService { get; set; } = default!;
        [Inject] private IFirebaseStorageService FirebaseStorageService { get; set; } = default!;
        [Inject] private IFirebaseConfigService FirebaseConfigService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;
        protected bool IsLoading { get; set; }
        protected List<Archivo> ListaArchivos { get; set; } = new();
        protected List<Categoria> ListaCategorias { get; set; } = new();
        protected string MensajeError { get; set; } = "";
        protected string MensajeOk { get; set; } = "";

        #region Flag por Rol
        protected bool PuedeCrear => Auth.EsSuper || Auth.EsAdmin || Auth.EsEmpleado || Auth.EsSupervisor;
        protected bool PuedeEditar => Auth.EsSuper || Auth.EsAdmin || Auth.EsSupervisor;
        protected bool PuedeEliminar => Auth.EsSuper || Auth.EsAdmin;
        #endregion

        #region Edición
        protected bool MostrarModal { get; set; } = false;
        protected bool EsNuevo { get; set; } = true;
        protected ArchivoForm ArchivoForm { get; set; } = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            ListaCategorias = await CategoriaService.ListarAsync();

            await CargarArchivos();
        }

        private async Task OnFileSelected(InputFileChangeEventArgs e)
        {
            MensajeError = "";
            var file = e.File;
            if (file == null) return;

            try
            {
                if (ArchivoForm.CategoriaId <= 0)
                {
                    MensajeError = "Primero seleccione una categoría.";
                    return;
                }
                using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB
                string nombreImagen = string.Empty;
                if (file != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(file.Name);
                    nombreImagen = nombre_en_codigo + extension;
                }

                var url = await FirebaseStorageService.SubirArchivoAsync(stream, ArchivoForm.CategoriaId, nombreImagen);
                if (string.IsNullOrWhiteSpace(url))
                {
                    MensajeError = "No se pudo obtener la URL del archivo en Firebase.";
                    return;
                }
                ArchivoForm.Nombre = nombreImagen;
                ArchivoForm.Url = url;
            }
            catch (Exception ex)
            {
                MensajeError = "Error al subir el archivo: " + ex.Message;
            }
        }
        private async Task CargarArchivos()
        {
            MensajeError = "";
            MensajeOk = "";

            ListaArchivos = await ArchivoService.ListarAsync();
        }

        protected string? NombreCategoriaPorId(int categoriaId)
        {
            return ListaCategorias.FirstOrDefault(c => c.IdCategoria == categoriaId)?.Nombre;
        }

        protected string NombreCategoria(int categoriaId)
        {
            return ListaCategorias
                .FirstOrDefault(c => c.IdCategoria == categoriaId)?.Nombre
                ?? "(Sin categoría)";
        }

        protected void NuevoArchivo()
        {
            EsNuevo = true;
            MensajeError = "";
            MensajeOk = "";
            ArchivoForm = new ArchivoForm();
            MostrarModal = true;
        }

        protected void EditarArchivo(Archivo archivo)
        {
            EsNuevo = false;
            MensajeError = "";
            MensajeOk = "";

            ArchivoForm = new ArchivoForm
            {
                IdArchivo = archivo.IdArchivo,
                Nombre = archivo.Nombre,
                Url = archivo.Url,
                CategoriaId = archivo.CategoriaId
            };
            MostrarModal = true;
        }

        private Archivo MapearAEntidad(ArchivoForm form)
        {
            return new Archivo
            {
                IdArchivo = form.IdArchivo,
                Nombre = form.Nombre,
                Url = form.Url,
                CategoriaId = form.CategoriaId
            };
        }

        protected void CerrarModal()
        {
            MostrarModal = false;
        }

        protected async Task GuardarArchivo()
        {
            MensajeError = "";
            MensajeOk = "";

            try
            {
                var entidad = MapearAEntidad(ArchivoForm);
                if (EsNuevo)
                {
                    entidad.UsuarioCrea = Auth.UsuarioActual?.IdUsuario ?? 0;
                    await ArchivoService.CrearAsync(entidad);
                    MensajeOk = "Archivo creado correctamente.";
                }
                else
                {
                    entidad.UsuarioActualiza = Auth.UsuarioActual?.IdUsuario ?? 0;
                    await ArchivoService.EditarAsync(entidad);
                    MensajeOk = "Archivo actualizado correctamente.";
                }

                MostrarModal = false;
                await CargarArchivos();
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
        }

        protected async Task EliminarArchivo(int idArchivo)
        {
            MensajeError = "";
            MensajeOk = "";

            var usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            var ok = await ArchivoService.EliminarLogicoAsync(idArchivo, usuarioId);

            if (!ok)
            {
                MensajeError = "No fue posible eliminar el archivo.";
                return;
            }

            MensajeOk = "Archivo eliminado correctamente.";
            await CargarArchivos();
        }
    }
}
