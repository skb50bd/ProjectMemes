using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XMemes.Models.Domain;
using XMemes.Models.Operations;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public interface IRepository<T>: IDisposable where T: BaseEntity {
        Task<T?> GetById(Guid id);
        Task<IList<T>> GetAll();
        Task<IPagedList<T>> Get<TKey>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy,
            bool descendingOrder,
            int pageIndex,
            int pageSize);

        IQueryable<T> GetQueryable();

        Task<IPagedList<T>> Search(string keyword, int pageIndex = 0, int pageSize = 20);

        Task<Outcome<T>> Insert(T item);
        Task<Outcome<T>> Update(T item);
        Task<Outcome<T>> Delete(T item);
        Task<bool> Exists(Guid id);
    }
}