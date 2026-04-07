using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IArchivoService
    {
        Task<List<Archivo>> ListarAsync();
        Task<Archivo?> ObtenerPorIdAsync(int idArchivo);
        Task<Archivo> CrearAsync(Archivo archivo);
        Task<Archivo> EditarAsync(Archivo archivo);
        Task<bool> EliminarLogicoAsync(int idArchivo, int usuarioActualizaId);
    }
}
