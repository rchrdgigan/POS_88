using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using POS_88.Business;

namespace POS_88.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context) { this.Context = context; }


        public TEntity Get(int id)
        {
            return this.Context.Set<TEntity>().Find(new object[1]
            {
                (object) id
            });
        }

        public IEnumerable<TEntity> GetAll()
        {
            return (IEnumerable<TEntity>)this.Context.Set<TEntity>().ToList<TEntity>();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return (IEnumerable<TEntity>)this.Context.Set<TEntity>().Where<TEntity>(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.Set<TEntity>().Where<TEntity>(predicate).Count<TEntity>() > 0;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.Set<TEntity>().SingleOrDefault<TEntity>(predicate);
        }

        public void Add(TEntity entity) => this.Context.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity) => this.Context.Set<TEntity>().Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this.Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
        }
    }


}
