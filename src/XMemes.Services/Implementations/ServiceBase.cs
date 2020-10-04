using AutoMapper;

using System;
using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.Paging;
using XMemes.Services.Abstractions;

namespace XMemes.Services.Implementations
{
    public abstract class ServiceBase<TData, TViewModel, TInput> 
        : IService<TViewModel, TInput> 
        where TData : TrackedEntity
        where TViewModel : class
        where TInput: BaseInput
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

        public virtual async Task<Outcome<TViewModel>> Insert(TInput model)
        {
            var dataModel = Mapper.Map<TData>(model);
            var outcome = await Repository.Insert(dataModel);
            return outcome.IsSuccess 
                ? Outcome<TViewModel>.FromSuccess(Mapper.Map<TViewModel>(outcome.Value)) 
                : Outcome<TViewModel>.FromError(outcome);
        }

        public virtual async Task<Outcome<TViewModel>> Update(TInput model)
        {
            var isValidId = Guid.TryParse(model.Id, out var id);
            if (!isValidId)
                return Outcome<TViewModel>.FromError("Id is not a valid guid");

            var dataModel = Mapper.Map<TData>(model);
            var outcome = await Repository.Update(dataModel);
            return outcome.IsSuccess 
                ? Outcome<TViewModel>.FromSuccess(Mapper.Map<TViewModel>(outcome.Value)) 
                : Outcome<TViewModel>.FromError(outcome);
        }

        public virtual async Task<Outcome<TViewModel>> Delete(Guid id)
        {
            var dataModel = await Repository.GetById(id);
            if (dataModel is null) 
                return Outcome<TViewModel>.FromError("Not Found.");
            var outcome = await Repository.Delete(dataModel);
            return outcome.IsSuccess
                ? Outcome<TViewModel>.FromSuccess(Mapper.Map<TViewModel>(dataModel))
                : Outcome<TViewModel>.FromError($"Error deleting item. Id: {id}");
        }

        public virtual async Task<bool> Exists(Guid id) =>
            await Repository.Exists(id);
    }
}