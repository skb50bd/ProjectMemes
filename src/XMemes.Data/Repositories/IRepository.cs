using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using XMemes.Models.Domain;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public interface IRepository<T>: IDisposable where T: BaseEntity {
        T? GetById(ObjectId id);
        IList<T> GetAll();
        IPagedList<T> Get(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);

        bool Insert(T item);
        bool Update(T item);
        bool Delete(T item);
    }
}