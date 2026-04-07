
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ListarAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario> CrearAsync(Usuario usuario, int usuarioCrea);
        Task<Usuario?> ActualizarAsync(Usuario usuario, int usuarioActualiza);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
        Task<Usuario?> ValidarCredencialesAsync(string correo, string clavePlano);
        Task<bool> EnviarClave(string Correo, string UrlPlantillaCorreo, int TipoCambio);
        Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo);
    }
}
