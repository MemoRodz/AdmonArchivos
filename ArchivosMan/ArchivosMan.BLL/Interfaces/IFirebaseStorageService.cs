namespace ArchivosMan.BLL.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> SubirStorage(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo);

        Task<string> SubirArchivoAsync(Stream streamArchivo, int categoriaId, string nombreArchivo);

        Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo);
    }
}
