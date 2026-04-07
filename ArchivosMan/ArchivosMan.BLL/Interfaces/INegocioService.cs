using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface INegocioService
    {
        Task<Negocio?> ActualizarAsync(Negocio negocio, int usuarioActualiza);
        Task<Negocio> CrearAsync(Negocio negocio, int usuarioCrea);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
        Task<List<Negocio>> ListarAsync();
        Task<Negocio?> ObtenerPorIdAsync(int id);
    }
}
