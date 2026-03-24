using ArchivosMan.BLL.Services;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IFirebaseConfigService
    {
        Task<FirebaseStorageConfig> ObtenerConfigAsync();
        Task<string> ObtenerCarpetaPorCategoriaAsync(string nombreCategoria);
    }
}
