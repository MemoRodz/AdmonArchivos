using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Estados
    {
        [Inject] private IEstadoService EstadoService { get; set; } = default!;
        [Inject] private IPaisService PaisService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;

        protected List<Estado> ListaEstados { get; set; } = new();
        protected List<Pais> ListaPaises { get; set; } = new();

        protected Estado? EstadoEditando { get; set; }
        protected bool IsLoading { get; set; }
        protected bool ModoEdicion { get; set; }
        #region Paginación
        protected int PaginaActual { get; set; } = 1;
        protected int TamanoPagina { get; set; } = 10;
        protected int TotalRegistros => ListaEstados?.Count ?? 0;
        protected int TotalPaginas =>
            TotalRegistros == 0 ? 1 : (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
        protected int TotalColumnasTabla => (PuedeEditar || PuedeEliminar) ? 4 : 3;
        protected List<Estado> ListaEstadosPaginada =>
            ListaEstados
            .Skip((PaginaActual - 1) * TamanoPagina)
            .Take(TamanoPagina)
            .ToList();
        protected List<int> PaginasVisibles
        {
            get
            {
                var total = TotalPaginas;
                var actual = PaginaActual;

                int inicio = Math.Max(1, actual - 2);
                int fin = Math.Min(total, inicio + 4);

                if ((fin - inicio) < 4)
                {
                    inicio = Math.Max(1, fin - 4);
                }

                return Enumerable.Range(inicio, fin - inicio + 1).ToList();
            }
        }
        protected void PaginaAnterior()
        {
            if (PaginaActual > 1)
                PaginaActual--;
        }
        protected void PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas)
                PaginaActual++;
        }
        protected void IrAPagina(int pagina)
        {
            if (pagina >= 1 && pagina <= TotalPaginas)
                PaginaActual = pagina;
        }
        protected Task OnTamanoPaginaChanged()
        {
            PaginaActual = 1;
            return Task.CompletedTask;
        }
        protected void AjustarPaginaSiEsNecesario()
        {
            if (PaginaActual > TotalPaginas)
                PaginaActual = TotalPaginas;

            if (PaginaActual < 1)
                PaginaActual = 1;
        }
        #endregion

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
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            IsLoading = true;
            try
            {
                MensajeError = "";
                MensajeOk = "";
                ListaPaises = await PaisService.ListarAsync();
                ListaEstados = await EstadoService.ListarAsync();
                AjustarPaginaSiEsNecesario();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void NuevoEstado()
        {
            try
            {
                var nuevo = new Estado
                {
                    IdEstado = 0,
                    Nombre = "",
                    PaisId = ListaPaises.FirstOrDefault()?.IdPais ?? 0
                };
                ListaEstados.Insert(0, nuevo);
                EstadoEditando = nuevo;
                ModoEdicion = true;
                PaginaActual = 1;
                MensajeOk = $"Nuevo Estado '{nuevo.Nombre}' creado.";
            }
            catch (Exception)
            {
                MensajeError = $"Error al crear Estado.";
            }

        }

        protected void EditarEstado(Estado estado)
        {
            EstadoEditando = estado;
            ModoEdicion = true;
        }

        protected void CancelarEdicion()
        {
            EstadoEditando = null;
            ModoEdicion = false;
        }

        protected async Task GuardarEdicion()
        {
            if (EstadoEditando == null) return;

            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;

            if (EstadoEditando.IdEstado == 0)
            {
                await EstadoService.CrearAsync(EstadoEditando, usuarioId);
            }
            else
            {
                await EstadoService.ActualizarAsync(EstadoEditando, usuarioId);
            }

            EstadoEditando = null;
            ModoEdicion = false;
            await CargarDatos();
        }

        protected async Task EliminarEstado(int id)
        {
            int usuarioId = Auth.UsuarioActual?.IdUsuario ?? 0;
            await EstadoService.EliminarAsync(id, usuarioId);
            await CargarDatos();
        }
    }
}
