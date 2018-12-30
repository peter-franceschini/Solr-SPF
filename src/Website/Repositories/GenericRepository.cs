using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Website.DataLayer;

namespace Website.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        public List<TEntity> GetAll()
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().ToList();
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>()
                   .Where(predicate).ToList();
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>()
                   .Where(predicate)
                   .Include(include)
                   .ToList();
            }
        }

        public TEntity GetById(object id)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().Find(id);
            }
        }

        public TEntity FirstOrDefault()
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().FirstOrDefault();
            }
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().FirstOrDefault(predicate);
            }
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>()
                    .Include(include)
                    .FirstOrDefault(predicate);
            }
        }

        public TEntity First()
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().First();
            }
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                return Context.Set<TEntity>().First(predicate);
            }
        }

        public void Delete(TEntity entity)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                    Context.Set<TEntity>().Attach(entity);
                Context.Set<TEntity>().Remove(entity);
                Context.SaveChanges();
            }
        }

        public void Insert(TEntity entity)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                Context.Set<TEntity>().Add(entity);
                Context.SaveChanges();
            }
        }

        public void Update(TEntity entity)
        {
            using (var Context = new SolrQueryExplainContext())
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();
            }
        }
    }
}
