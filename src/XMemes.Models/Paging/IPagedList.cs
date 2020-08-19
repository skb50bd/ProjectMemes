using System.Collections.Generic;

namespace XMemes.Models.Paging
{
    public interface IPagedList<T>: IList<T>, IPaginationProperties { }
}