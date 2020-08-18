using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using XMemes.Models.Domain;

namespace XMemes.Data.Repositories
{
    public interface IRepository<T>: IDisposable where T: BaseEntity {
        T? GetById(ObjectId id);
        IList<T> GetAll();
        IList<T> Get(Expression<Func<T, bool>> predicate);
        
        bool Insert(T item);
        bool Update(T item);
        bool Delete(T item);
    }
}