using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Perfil
    {
        [Inject] private AuthState Auth { get; set; } = default!;
        [Inject] private IFirebaseStorageService FirebaseStorageService { get; set; } = default!;
        [Inject] private IFirebaseConfigService FirebaseConfigService { get; set; } = default!;
        [Inject] private IUsuarioService UsuarioService { get; set; } = default!;
        [Inject] private IArchivoService ArchivoService { get; set; } = default!;

        private string MensajeErrorFoto { get; set; } = string.Empty;
        private string MensajeOkFoto { get; set; } = string.Empty;
        private string NombreEditable { get; set; } = string.Empty;
        private string MensajeErrorNombre { get; set; } = string.Empty;
        private string MensajeOkNombre { get; set; } = string.Empty;

        private const int CategoriaFotoUsuarioId = Constantes.CatImagenes.UsuarioPic;

        protected override void OnInitialized()
        {
            if (Auth.UsuarioActual is not null)
                NombreEditable = Auth.UsuarioActual.Nombre;
        }

        private async Task GuardarNombre()
        {
            MensajeErrorNombre = string.Empty;
            MensajeOkNombre = string.Empty;

            if (Auth.UsuarioActual is null)
            {
                MensajeErrorNombre = "No hay usuario autenticado.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NombreEditable))
            {
                MensajeErrorNombre = "El nombre no puede estar vacío.";
                return;
            }

            if (NombreEditable.Trim() == Auth.UsuarioActual.Nombre)
            {
                MensajeErrorNombre = "El nombre es igual al actual, no hay cambios.";
                return;
            }

            try
            {
                var usuario = new Usuario
                {
                    IdUsuario = Auth.UsuarioActual.IdUsuario,
                    Nombre = NombreEditable.Trim(),
                    Correo = Auth.UsuarioActual.Correo,
                    RolId = Auth.UsuarioActual.RolId,
                    UrlFoto = Auth.UsuarioActual.UrlFoto,
                    Clave = "" 
                };

                var actualizado = await UsuarioService.ActualizarAsync(
                    usuario,
                    Auth.UsuarioActual.IdUsuario);

                if (actualizado is null)
                {
                    MensajeErrorNombre = "No fue posible actualizar el nombre.";
                    return;
                }

                Auth.UsuarioActual.Nombre = actualizado.Nombre;

                MensajeOkNombre = "Nombre actualizado correctamente.";
            }
            catch (Exception ex)
            {
                MensajeErrorNombre = "Error al actualizar el nombre: " + ex.Message;
            }
        }

        private async Task OnFotoSeleccionada(InputFileChangeEventArgs e)
        {
            MensajeErrorFoto = string.Empty;
            MensajeOkFoto = string.Empty;

            if (Auth.UsuarioActual is null)
            {
                MensajeErrorFoto = "No hay usuario autenticado.";
                return;
            }

            var file = e.File;
            if (file == null) return;

            try
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);

                string nombreEnCodigo = Guid.NewGuid().ToString("N");
                string extension = Path.GetExtension(file.Name);
                string nombreImagen = nombreEnCodigo + extension;

                var url = await FirebaseStorageService
                    .SubirArchivoAsync(stream, CategoriaFotoUsuarioId, nombreImagen);

                if (string.IsNullOrWhiteSpace(url))
                {
                    MensajeErrorFoto = "No se pudo obtener la URL de la foto en Firebase.";
                    return;
                }

                var archivo = new Archivo
                {
                    Nombre = nombreImagen,
                    Url = url,
                    CategoriaId = CategoriaFotoUsuarioId,
                    EsActivo = true,
                    FechaCrea = DateTime.Now,
                    UsuarioCrea = Auth.UsuarioActual.IdUsuario
                };

                var archivoCreado = await ArchivoService.CrearAsync(archivo);

                var usuario = new Usuario
                {
                    IdUsuario = Auth.UsuarioActual.IdUsuario,
                    Nombre = Auth.UsuarioActual.Nombre,
                    Correo = Auth.UsuarioActual.Correo,
                    RolId = Auth.UsuarioActual.RolId,
                    UrlFoto = archivoCreado.Url,
                    Clave = "" 
                };

                var actualizado = await UsuarioService.ActualizarAsync(
                    usuario,
                    Auth.UsuarioActual.IdUsuario);

                if (actualizado is null)
                {
                    MensajeErrorFoto = "No fue posible actualizar la foto.";
                    return;
                }

                Auth.UsuarioActual.UrlFoto = actualizado.UrlFoto;

                MensajeOkFoto = "Foto actualizada correctamente.";
            }
            catch (Exception ex)
            {
                MensajeErrorFoto = "Error al subir la foto: " + ex.Message;
            }
        }
    }
}
