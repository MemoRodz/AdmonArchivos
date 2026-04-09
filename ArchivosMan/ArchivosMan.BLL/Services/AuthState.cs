using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Services
{
    public class AuthState
    {
        public Usuario? UsuarioActual { get; private set; }

        public bool EstaAutenticado => UsuarioActual != null;

        public event Action? OnChange;

        public void EstablecerUsuario(Usuario usuario)
        {
            UsuarioActual = usuario;
            NotifyStateChanged();
        }

        public void CerrarSesion() => UsuarioActual = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public bool EsSuper => TieneRol("Super");
        public bool EsAdmin => TieneRol("Administrador");
        public bool EsSupervisor => TieneRol("Supervisor");
        public bool EsEmpleado => TieneRol("Empleado");
        public bool EsContacto => TieneRol("Contacto");

        public bool TieneRol(params string[] nombresRol)
        {
            var rol = UsuarioActual?.Rol?.Descripcion;
            if (string.IsNullOrWhiteSpace(rol)) return false;

            return nombresRol.Any(r =>
                string.Equals(r, rol, StringComparison.OrdinalIgnoreCase));
        }
    }
}
