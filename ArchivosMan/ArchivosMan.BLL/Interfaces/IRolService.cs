using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> ListarAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
    }
}
