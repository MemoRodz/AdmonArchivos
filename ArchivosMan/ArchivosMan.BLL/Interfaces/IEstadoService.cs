using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IEstadoService
    {
        Task<List<Estado>> ListarAsync();
        Task<Estado?> ObtenerPorIdAsync(int id);
        Task<List<Estado>> ListarPorPaisAsync(int paisId);
        Task<Estado> CrearAsync(Estado estado, int usuarioCrea);
        Task<Estado?> ActualizarAsync(Estado estado, int usuarioActualiza);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
    }
}
