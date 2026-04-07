using Microsoft.EntityFrameworkCore;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Implementacion
{
    public class ArchivoService : IArchivoService
    {
        private readonly IGenericRepository<Archivo> _repoArchivo;
        public ArchivoService(IGenericRepository<Archivo> repoArchivo)
        {
            _repoArchivo = repoArchivo;
        }
        public async Task<Archivo> CrearAsync(Archivo archivo)
        {
            archivo.EsActivo = true;
            archivo.FechaCrea = DateTime.Now;

            return await _repoArchivo.Crear(archivo);
        }

        public async Task<Archivo> EditarAsync(Archivo archivo)
        {
            var archivoDb = await _repoArchivo.Obtener(a => a.IdArchivo == archivo.IdArchivo && a.EsActivo)
                ?? throw new InvalidOperationException("El archivo no existe o está inactivo.");

            archivoDb.Nombre = archivo.Nombre;
            archivoDb.Url = archivo.Url;
            archivoDb.CategoriaId = archivo.CategoriaId;
            archivoDb.FechaActualiza = DateTime.Now;
            archivoDb.UsuarioActualiza = archivo.UsuarioActualiza;

            await _repoArchivo.Editar(archivoDb);
            return archivoDb;
        }

        public async Task<bool> EliminarLogicoAsync(int idArchivo, int usuarioActualizaId)
        {
            var archivoDb = await _repoArchivo.Obtener(a => a.IdArchivo == idArchivo && a.EsActivo);
            if (archivoDb == null) return false;

            archivoDb.EsActivo = false;
            archivoDb.FechaActualiza = DateTime.Now;
            archivoDb.UsuarioActualiza = usuarioActualizaId;

            await _repoArchivo.Editar(archivoDb);
            return true;
        }

        public async Task<List<Archivo>> ListarAsync()
        {
            return await _repoArchivo.Consultar(a => a.EsActivo)
                .OrderByDescending(a => a.FechaCrea)
                .ToListAsync();
        }

        public async Task<Archivo?> ObtenerPorIdAsync(int idArchivo)
        {
            return await _repoArchivo.Obtener(a => a.IdArchivo == idArchivo && a.EsActivo);
        }
    }
}
