using System;
using System.Collections.Generic;

namespace XMemes.Models.Paging
{
    public class PagedList<T>: List<T>, IPagedList<T>
    {
        protected PagedList() {}

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalCount = totalCount;
            AddRange(source);
        }

        public static PagedList<T> Empty() => new PagedList<T>();

        public int PageIndex { get; }
        public int PageSize { get; }
        
        public int TotalCount { get; }

        public int TotalPages =>
            (int)Math.Ceiling(TotalCount / (float)PageSize);

        public bool HasNextPage =>
            PageIndex >= 0
            && TotalPages - 1 > PageIndex;

        public bool HasPreviousPage =>
            PageIndex > 0;
    }
}
