using System.Linq.Expressions;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Remove(TEntity entity);
    }
}