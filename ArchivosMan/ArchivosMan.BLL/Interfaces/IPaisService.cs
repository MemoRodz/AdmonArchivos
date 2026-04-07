
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IPaisService
    {
        Task<Pais?> ActualizarAsync(Pais pais, int usuarioActualiza);
        Task<Pais> CrearAsync(Pais pais, int usuarioCrea);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
        Task<List<Pais>> ListarAsync();
        Task<Pais?> ObtenerPorIdAsync(int id);
    }
}
