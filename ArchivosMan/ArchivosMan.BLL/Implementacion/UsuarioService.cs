
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ArchivosMan.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ArchivosContext _db;
        private readonly ICryptoService _crypto;
        private readonly ICorreoService _correoService;
        private readonly IUtilidadesService _utilidadesService;

        public UsuarioService(ArchivosContext db, ICryptoService crypto, ICorreoService correoService, IUtilidadesService utilidadesService)
        {
            _db = db;
            _crypto = crypto;
            _correoService = correoService;
            _utilidadesService = utilidadesService;
        }
        public async Task<Usuario?> ActualizarAsync(Usuario usuario, int usuarioActualiza)
        {
            var existente = await _db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);
            if (existente == null) return null;

            if (!string.Equals(existente.Correo, usuario.Correo, StringComparison.OrdinalIgnoreCase))
            {
                bool existeCorreo = await _db.Usuarios
                    .AnyAsync(u => u.Correo == usuario.Correo && u.IdUsuario != usuario.IdUsuario && u.EsActivo);
                if (existeCorreo)
                    throw new InvalidOperationException("Ya existe un usuario con ese correo.");
            }

            existente.Nombre = usuario.Nombre;
            existente.Correo = usuario.Correo;
            existente.RolId = usuario.RolId;
            existente.UrlFoto = usuario.UrlFoto;
            existente.FechaActualiza = DateTime.Now;
            existente.UsuarioActualiza = usuarioActualiza;

            if (!string.IsNullOrWhiteSpace(usuario.Clave))
            {
                existente.Clave = _crypto.Encrypt(usuario.Clave);
            }

            await _db.SaveChangesAsync();
            return existente;
        }

        public async Task<Usuario> CrearAsync(Usuario usuario, int usuarioCrea)
        {
            bool existeCorreo = await _db.Usuarios
                .AnyAsync(u => u.Correo == usuario.Correo && u.EsActivo);

            if (existeCorreo)
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");

            usuario.EsActivo = true;
            usuario.FechaCrea = DateTime.Now;
            usuario.UsuarioCrea = usuarioCrea;

            // Encriptar clave AES256
            usuario.Clave = _crypto.Encrypt(usuario.Clave);

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null) return false;

            usuario.EsActivo = false;
            usuario.FechaActualiza = DateTime.Now;
            usuario.UsuarioActualiza = usuarioActualiza;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnviarClave(string Correo, string UrlPlantillaCorreo, int TipoCambio)
        {
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == Correo && u.EsActivo);

            if (usuario == null)
                throw new InvalidOperationException($"No se encontró ningún usuario asociado al correo '{Correo}'.");

            var claveAct = _crypto.Decrypt(usuario.Clave);

            if (!File.Exists(UrlPlantillaCorreo))
                throw new FileNotFoundException("No se encontró la plantilla de correo.", UrlPlantillaCorreo);

            string htmlCorreo = File.ReadAllText(UrlPlantillaCorreo, Encoding.UTF8);

            htmlCorreo = htmlCorreo.Replace("{{CLAVE}}", claveAct);
            switch (TipoCambio)
            {
                case 1:
                    htmlCorreo = htmlCorreo.Replace("{{TIPOCAMBIO}}", Constantes.TipoPlantilla.Actualiza);
                    break;
                case 2:
                    htmlCorreo = htmlCorreo.Replace("{{TIPOCAMBIO}}", Constantes.TipoPlantilla.Crea);
                    break;
                default:
                    htmlCorreo = htmlCorreo.Replace("{{TIPOCAMBIO}}", string.Empty);
                    break;

            }

            bool correoEnviado = await _correoService.EnviarCorreo(
                usuario.Correo,
                "Contraseña actualizada.",
                htmlCorreo);

            if (!correoEnviado)
                throw new InvalidOperationException("Tenemos problemas. Por favor inténtalo de nuevo más tarde.");

            return true;
        }

        public async Task<List<Usuario>> ListarAsync()
        {
            return await _db.Usuarios
                .Include(u => u.Rol)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _db.Usuarios
                .Include(u => u.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo)
        {
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == Correo && u.EsActivo);

            if (usuario == null)
                throw new InvalidOperationException($"No se encontró ningún usuario asociado al correo '{Correo}'.");

            var claveGenerada = _utilidadesService.GenerarClave();

            usuario.Clave = _crypto.Encrypt(claveGenerada);
            usuario.FechaActualiza = DateTime.Now;
            usuario.UsuarioActualiza = Constantes.Usuario.Correo;

            if (!File.Exists(UrlPlantillaCorreo))
                throw new FileNotFoundException("No se encontró la plantilla de correo.", UrlPlantillaCorreo);

            string htmlCorreo = File.ReadAllText(UrlPlantillaCorreo, Encoding.UTF8);

            htmlCorreo = htmlCorreo.Replace("{{CLAVE}}", claveGenerada);
            htmlCorreo = htmlCorreo.Replace("{{TIPOCAMBIO}}", Constantes.TipoPlantilla.Restablece);

            bool correoEnviado = await _correoService.EnviarCorreo(
                usuario.Correo,
                "Contraseña restablecida.",
                htmlCorreo);

            if (!correoEnviado)
                throw new InvalidOperationException("Tenemos problemas. Por favor inténtalo de nuevo más tarde.");

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string correo, string clavePlano)
        {
            var usuario = await _db.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.EsActivo);

            if (usuario == null) return null;

            var claveDesencriptada = _crypto.Decrypt(usuario.Clave);
            return claveDesencriptada == clavePlano ? usuario : null;
        }
    }
}
