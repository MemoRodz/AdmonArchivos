using System.Linq.Expressions;

namespace ArchivosMan.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Consultar(Expression<Func<TEntity, bool>>? filtro = null);
        Task<IQueryable<TEntity>> ConsultaAsync(Expression<Func<TEntity, bool>> filtro = null);
        Task<TEntity?> Obtener(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> Crear(TEntity entidad);
        Task Editar(TEntity entidad);
    }
}
