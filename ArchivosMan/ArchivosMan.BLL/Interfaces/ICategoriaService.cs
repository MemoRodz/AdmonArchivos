using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ListarAsync();
    }
}
