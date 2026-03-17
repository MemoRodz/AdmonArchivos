using Microsoft.EntityFrameworkCore;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.DAL.Interfaces;
using System.Linq.Expressions;

namespace ArchivosMan.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ArchivosContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ArchivosContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public Task<IQueryable<TEntity>> ConsultaAsync(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(filtro);
            return (Task<IQueryable<TEntity>>)queryEntidad;
        }

        public IQueryable<TEntity> Consultar(Expression<Func<TEntity, bool>>? filtro = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filtro != null)
                query = query.Where(filtro);

            return query;
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }

        public async Task Editar(TEntity entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity?> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            return await _dbSet.FirstOrDefaultAsync(filtro);
        }
    }
}
