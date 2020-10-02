using System;
using System.Threading.Tasks;
using XMemes.Models.Paging;

namespace XMemes.Services.Abstractions
{
    public interface IService<TViewModel, in TInput>
    {
        Task<IPagedList<TViewModel>> GetAll(
            int pageIndex = 0,
            int pageSize = 20);

        Task<TViewModel> GetById(Guid id);

        Task<IPagedList<TViewModel>> Search(
            string keyword,
            int pageIndex = 0,
            int pageSize = 20);

        Task<bool> Insert(TInput model);
        Task<bool> Update(TInput model);
        Task<bool> Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}