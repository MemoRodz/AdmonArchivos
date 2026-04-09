using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Negocios
    {
        [Inject] private INegocioService NegocioService { get; set; } = default!;
        [Inject] private IArchivoService ArchivoService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;
        [Inject] private IFirebaseStorageService FirebaseStorageService { get; set; } = default!;
        [Inject] private IFirebaseConfigService FirebaseConfigService { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        protected List<Negocio> ListaNegocios { get; set; } = new();
        protected List<Archivo> ListaArchivos { get; set; } = new();
        protected Negocio? NegocioEditando { get; set; }
        protected bool IsLoading { get; set; }
        protected bool ModoEdicion { get; set; }
        protected string FiltroNegocio { get; set; } = string.Empty;
        protected Negocio? NegocioSeleccionado { get; set; }

        private static readonly string[] ExtensionesPermitidas =
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        protected int? NegocioSeleccionadoId
        {
            get => NegocioSeleccionado?.IdNegocio;
            set
            {
                if (value.HasValue)
                {
                    NegocioSeleccionado = ListaNegocios
                        .FirstOrDefault(n => n.IdNegocio == value.Value);
                }
                else
                {
                    NegocioSeleccionado = null;
                }
            }
        }

        protected IEnumerable<Negocio> NegociosFiltrados =>
            string.IsNullOrWhiteSpace(FiltroNegocio)
            ? ListaNegocios
            : ListaNegocios.Where(n =>
            (!string.IsNullOrWhiteSpace(n.Nombre) &&
            n.Nombre.Contains(FiltroNegocio, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(n.NumeroDocumento) &&
            n.NumeroDocumento.Contains(FiltroNegocio, StringComparison.OrdinalIgnoreCase)));

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
            if (!Auth.EstaAutenticado)
            {
                Nav.NavigateTo("/login", true);
                return;
            }

            if (!(Auth.EsSuper || Auth.EsAdmin))
            {
                Nav.NavigateTo("/", true);
            }

            await CargarNegocios();
        }

        private async Task CargarNegocios()
        {
            IsLoading = true;
            try
            {
                MensajeError = string.Empty;
                MensajeOk = string.Empty;
                ListaNegocios = await NegocioService.ListarAsync();
                if (ListaNegocios.Any() && NegocioSeleccionado == null)
                {
                    NegocioSeleccionado = ListaNegocios.First();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void SeleccionarNegocio(int id)
        {
            NegocioSeleccionado = ListaNegocios.FirstOrDefault(n => n.IdNegocio == id);
            FiltroNegocio = string.Empty;
        }

        protected void NuevoNegocio()
        {
            try
            {
                MensajeError = string.Empty;
                MensajeOk = string.Empty;

                var nuevo = new Negocio
                {
                    IdNegocio = 0,
                    Nombre = "",
                    UrlLogo = "",
                    NombreLogo = "",
                    Direccion = "",
                    Codpos = "",
                    Pais = "",
                    Telefono = "",
                    PorcentajeImpuesto = 0.0M,
                    SimboloMoneda = "",
                    NumeroDocumento = ""
                };

                ListaNegocios?.Insert(0, nuevo);
                NegocioSeleccionado = nuevo;

                MensajeOk = $"Nuevo Negocio '{nuevo.Nombre}' creado.";
            }
            catch (Exception)
            {
                MensajeError = $"Error al crear Negocio.";
            }
        }

        protected void EditarNegocio(Negocio negocio)
        {
            NegocioEditando = negocio;
            ModoEdicion = true;
        }

        protected void CancelarEdicion()
        {
            NegocioEditando = null;
            ModoEdicion = false;
        }

        protected async Task GuardarEdicion()
        {
            if (NegocioEditando == null) return;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;

            if (NegocioEditando.IdNegocio == 0)
            {
                await NegocioService.CrearAsync(NegocioEditando, usuarioId);
            }
            else
            {
                await NegocioService.ActualizarAsync(NegocioEditando, usuarioId);
            }
            NegocioEditando = null;
            ModoEdicion = false;
            await CargarNegocios();
        }

        private bool ValidarNegocio(Negocio n)
        {
            if (string.IsNullOrWhiteSpace(n.Nombre))
            {
                MensajeError = "La razón social es obligatoria.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.Correo))
            {
                MensajeError = "El correo es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.Direccion))
            {
                MensajeError = "La dirección es obligatoria.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.Telefono))
            {
                MensajeError = "El teléfono es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.SimboloMoneda))
            {
                MensajeError = "El símbolo de moneda es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.Codpos))
            {
                MensajeError = "El código postal es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.Pais))
            {
                MensajeError = "El país es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.NumeroDocumento))
            {
                MensajeError = "El número de documento es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(n.PorcentajeImpuesto.ToString()))
            {
                MensajeError = "El porcentaje de impuesto es obligatorio.";
                return false;
            }
            return true;
        }

        protected async Task GuardarSeleccionado()
        {

            if (NegocioSeleccionado is null) return;

            MensajeError = string.Empty;
            MensajeOk = string.Empty;

            if (!ValidarNegocio(NegocioSeleccionado))
                return;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            int negocioIdGuardado;

            try
            {
                if (NegocioSeleccionado.IdNegocio == 0)
                {
                    var creado = await NegocioService.CrearAsync(NegocioSeleccionado, usuarioId);
                    negocioIdGuardado = creado.IdNegocio;
                    MensajeOk = $"Negocio '{NegocioSeleccionado.Nombre}' creado correctamente.";
                }
                else
                {
                    await NegocioService.ActualizarAsync(NegocioSeleccionado, usuarioId);
                    negocioIdGuardado = NegocioSeleccionado.IdNegocio;
                    MensajeOk = $"Negocio '{NegocioSeleccionado.Nombre}' actualizado correctamente.";
                }
                await CargarNegocios();
                NegocioSeleccionado = ListaNegocios
                    .FirstOrDefault(n => n.IdNegocio == negocioIdGuardado);
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar negocio: {ex.Message}";
            }
        }

        protected async Task EliminarNegocio(int id)
        {
            MensajeError = string.Empty;
            MensajeOk = string.Empty;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;

            var indexActual = ListaNegocios.FindIndex(n => n.IdNegocio == id);
            try
            {
                await NegocioService.EliminarAsync(id, usuarioId);
                MensajeOk = "Negocio eliminado correctamente.";
                await CargarNegocios();
                if (!ListaNegocios.Any())
                {
                    NegocioSeleccionado = null;
                }

                int nuevoIndex;

                if (indexActual >= ListaNegocios.Count)
                {
                    nuevoIndex = ListaNegocios.Count - 1;
                }
                else
                {
                    nuevoIndex = indexActual;
                }

                if (nuevoIndex < 0) nuevoIndex = 0;

                NegocioSeleccionado = ListaNegocios[nuevoIndex];
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar negocio: {ex.Message}";
            }
        }

        private async Task OnLogoSeleccionado(InputFileChangeEventArgs e)
        {
            MensajeError = string.Empty;
            MensajeOk = string.Empty;
            if (NegocioSeleccionado is null)
            {
                MensajeError = "Primero seleccione un negocio.";
                return;
            }


            var file = e.File;
            if (file == null) return;

            string extension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (!ExtensionesPermitidas.Contains(extension))
            {
                MensajeError = $"Tipo de archivo no permitido. Use: {string.Join(", ", ExtensionesPermitidas)}";
                return;
            }

            // Tamaño máximo (2MB para logos)
            const long tamañoMaximo = 2 * 1024 * 1024;
            if (file.Size > tamañoMaximo)
            {
                MensajeError = "El archivo no debe superar 2MB.";
                return;
            }

            try
            {
                const int categoriaLogoId = Constantes.CatImagenes.LogoPic;
                using var stream = file.OpenReadStream(maxAllowedSize: tamañoMaximo);

                string nombreEnCodigo = Guid.NewGuid().ToString("N");
                string nombreImagen = nombreEnCodigo + extension;

                var url = await FirebaseStorageService
                    .SubirArchivoAsync(stream, categoriaLogoId, nombreImagen);

                if (string.IsNullOrEmpty(url))
                {
                    MensajeError = "No se pudo obtener la URL del logo en Firebase.";
                    return;
                }

                var archivo = new Archivo
                {
                    Nombre = nombreImagen,
                    Url = url,
                    CategoriaId = categoriaLogoId,
                    EsActivo = true,
                    FechaCrea = DateTime.Now,
                    UsuarioCrea = Auth.UsuarioActual?.IdUsuario ?? 0
                };

                var archivoCreado = await ArchivoService.CrearAsync(archivo);

                NegocioSeleccionado.UrlLogo = archivoCreado.Url;
                NegocioSeleccionado.NombreLogo = archivoCreado.Nombre;

                MensajeOk = "Logo actualizado. No olvides guardar el negocio.";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al subir el logo: {ex.Message}";
            }
        }
    }
}
