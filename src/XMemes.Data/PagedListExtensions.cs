using LiteDB;
using XMemes.Models.Paging;

namespace XMemes.Data
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(
            this ILiteQueryable<T> source,
            int pageIndex,
            int pageSize)
        {
            var totalCount = source.Count();
            
            if (totalCount == 0) 
                return PagedList<T>.Empty();

            var items = source.Skip(pageIndex * pageSize).Limit(pageSize).ToList();
            return new PagedList<T>(items, pageIndex, pageSize, totalCount);
        }
    }
}