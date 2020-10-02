using MongoDB.Driver;

using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using XMemes.Models.Paging;

namespace XMemes.Data
{
    public static class PagedListExtensions
    {
        public static async Task<IPagedList<T>> ToPagedList<T>(
            this IFindFluent<T, T> source,
            int pageIndex,
            int pageSize)
        {
            var totalCount =
                await source.CountDocumentsAsync();

            if (totalCount == 0) return PagedList<T>.Empty();

            var items =
                await source
                    .Skip(pageIndex * pageSize)
                    .Limit(pageSize)
                    .ToListAsync();

            return new PagedList<T>(
                items,
                pageIndex,
                pageSize,
                (int)totalCount);
        }

        public static async Task<IPagedList<T>> ToPagedList<T>(
            this IMongoQueryable<T> source,
            int pageIndex,
            int pageSize)
        {
            var totalCount =
                await source.CountAsync();

            if (totalCount == 0) return PagedList<T>.Empty();

            var items =
                await source
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedList<T>(
                items,
                pageIndex,
                pageSize,
                totalCount);
        }
    }
}