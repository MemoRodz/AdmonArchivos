using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.Entity;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Usuarios
    {
        [Inject] private IUsuarioService UsuarioService { get; set; } = default!;
        [Inject] private IRolService RolService { get; set; } = default!;
        [Inject] private AuthState Auth { get; set; } = default!;


        protected List<Usuario> ListaUsuarios { get; set; } = new();
        protected List<Rol> ListaRoles { get; set; } = new();

        protected Usuario? UsuarioEditando { get; set; }
        protected string ClaveTemporal { get; set; } = ""; 
        protected bool IsLoading { get; set; }
        protected bool ModoEdicion { get; set; }
        private bool PuedeVerPagina => Auth.EsSuper || Auth.EsAdmin;
        protected IEnumerable<Rol> RolesPermitidos =>
            Auth.EsSuper
        ? ListaRoles
        : ListaRoles.Where(r => r.IdRol != Constantes.Usuario.Super);

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            IsLoading = true;
            try
            {
                ListaRoles = await RolService.ListarAsync();
                var todosUsuarios = await UsuarioService.ListarAsync();
                if (Auth.EsAdmin && !Auth.EsSuper)
                {
                    ListaUsuarios = todosUsuarios
                        .Where(u => u.RolId != Constantes.Usuario.Super)
                        .ToList();
                }
                else
                {
                    ListaUsuarios = todosUsuarios;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void NuevoUsuario()
        {
            var rolDefault = ListaRoles.FirstOrDefault();
            var nuevo = new Usuario
            {
                IdUsuario = 0,
                Nombre = "",
                Correo = "",
                RolId = rolDefault?.IdRol ?? 0,
                UrlFoto = ""
            };

            ListaUsuarios.Insert(0, nuevo);
            UsuarioEditando = nuevo;
            ClaveTemporal = "";
            ModoEdicion = true;
        }

        protected void EditarUsuario(Usuario usuario)
        {
            UsuarioEditando = usuario;
            ClaveTemporal = ""; // si se llena, se actualizará la clave
            ModoEdicion = true;
        }

        protected void CancelarEdicion()
        {
            UsuarioEditando = null;
            ClaveTemporal = "";
            ModoEdicion = false;
        }

        protected async Task GuardarEdicion()
        {
            if (UsuarioEditando == null) return;

            const int usuarioId = Constantes.Usuario.Admin; // por ahora fijo

            // Asignar clave desde UI solo si se capturó algo
            if (!string.IsNullOrWhiteSpace(ClaveTemporal))
            {
                UsuarioEditando.Clave = ClaveTemporal;
            }

            if (UsuarioEditando.IdUsuario == 0)
            {
                await UsuarioService.CrearAsync(UsuarioEditando, usuarioId);
            }
            else
            {
                await UsuarioService.ActualizarAsync(UsuarioEditando, usuarioId);
            }

            UsuarioEditando = null;
            ClaveTemporal = "";
            ModoEdicion = false;

            await CargarDatos();
        }

        protected async Task EliminarUsuario(int id)
        {
            const int usuarioId = 1;
            await UsuarioService.EliminarAsync(id, usuarioId);
            await CargarDatos();
        }
    }
}
