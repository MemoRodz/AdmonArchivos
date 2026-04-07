
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class RolService : IRolService
    {
        private readonly IDbContextFactory<ArchivosContext> _dbFactory;

        public RolService(IDbContextFactory<ArchivosContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<List<Rol>> ListarAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Roles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }
    }
}
