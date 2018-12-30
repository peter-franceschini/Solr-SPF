using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Website.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        List<TEntity> GetAll();

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(object id);

        TEntity FirstOrDefault();

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity First();

        TEntity First(Expression<Func<TEntity, bool>> predicate);

        void Delete(TEntity entity);

        void Insert(TEntity entity);

        void Update(TEntity entity);
    }
}
