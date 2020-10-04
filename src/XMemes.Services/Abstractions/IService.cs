using System;
using System.Threading.Tasks;
using XMemes.Models.Operations;
using XMemes.Models.Paging;

namespace XMemes.Services.Abstractions
{
    public interface IService<TViewModel, in TInput> where TViewModel: class
    {
        Task<IPagedList<TViewModel>> GetAll(
            int pageIndex = 0,
            int pageSize = 20);

        Task<TViewModel> GetById(Guid id);

        Task<IPagedList<TViewModel>> Search(
            string keyword,
            int pageIndex = 0,
            int pageSize = 20);

        Task<Outcome<TViewModel>> Insert(TInput model);
        Task<Outcome<TViewModel>> Update(TInput model);
        Task<Outcome<TViewModel>> Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}