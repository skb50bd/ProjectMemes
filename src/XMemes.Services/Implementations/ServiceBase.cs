using AutoMapper;

using System;
using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using XMemes.Models.Paging;
using XMemes.Services.Abstractions;

namespace XMemes.Services.Implementations
{
    public abstract class ServiceBase<TData, TViewModel, TInput> 
        : IService<TViewModel, TInput> where TData : TrackedEntity
    {
        protected readonly IRepository<TData> Repository;
        protected readonly IMapper Mapper;

        protected ServiceBase(
            IRepository<TData> repository,
            IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public abstract Task<IPagedList<TViewModel>> GetAll(
            int pageIndex = 0,
            int pageSize = 20);

        public virtual async Task<TViewModel> GetById(Guid id)
        {
            var dataModel = await Repository.GetById(id);
            var viewModel = Mapper.Map<TViewModel>(dataModel);
            return viewModel;
        }

        public virtual async Task<IPagedList<TViewModel>> Search(
            string keyword,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var dataModels = await Repository.Search(keyword, pageIndex, pageSize);
            return Mapper.Map<IPagedList<TViewModel>>(dataModels);
        }

        public virtual async Task<bool> Insert(TInput model)
        {
            var dataModel = Mapper.Map<TData>(model);
            return await Repository.Insert(dataModel);
        }

        public virtual async Task<bool> Update(TInput model)
        {
            var dataModel = Mapper.Map<TData>(model);
            return await Repository.Update(dataModel);
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            var dataModel = await Repository.GetById(id);
            if (dataModel is null) return false;
            return await Repository.Delete(dataModel);
        }

        public virtual async Task<bool> Exists(Guid id) =>
            await Repository.Exists(id);
    }
}