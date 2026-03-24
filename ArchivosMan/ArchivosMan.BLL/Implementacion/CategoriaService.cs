using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class CategoriaService :ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repoCategoria;

        public CategoriaService(IGenericRepository<Categoria> repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public async Task<List<Categoria>> ListarAsync()
        {
            return await _repoCategoria.Consultar(c => c.EsActivo).OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}
