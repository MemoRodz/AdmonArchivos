
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IProyectoService
    {
        Task<Proyecto?> ActualizarAsync(Proyecto proyecto, int usuarioActualiza);
        Task<Proyecto> CrearAsync(Proyecto proyecto, int usuarioCrea);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
        Task<List<Proyecto>> ListarAsync();
        Task<Proyecto?> ObtenerPorIdAsync(int id);
    }
}
