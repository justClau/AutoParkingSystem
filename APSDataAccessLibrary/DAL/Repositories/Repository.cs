using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
        }
        public TEntity Get(int id)
        {
            return context.Set<TEntity>().Find(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }
        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
    }
}
