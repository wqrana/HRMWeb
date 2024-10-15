using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TimeAide.Web.DAL.RepositoryInterfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
       
        TEntity Get(long id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
                
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        //void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        //void RemoveRange(IEnumerable<TEntity> entities);

    }
}