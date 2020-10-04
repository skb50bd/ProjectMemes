using AutoMapper;

using System;
using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.Paging;
using XMemes.Models.Utils;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Services.Implementations
{
    public class MemerService : ServiceBase<Memer, MemerViewModel, MemerInput>, IMemerService
    {
        private readonly IMemerRepository _memerRepository;
        private readonly IMapper _mapper;

        public MemerService(
            IMemerRepository memerRepository,
            IMapper mapper)
            : base(memerRepository, mapper)
        {
            _memerRepository = memerRepository;
            _mapper = mapper;
        }

        public override async Task<IPagedList<MemerViewModel>> GetAll(
            int pageIndex = 0,
            int pageSize = 20)
        {
            var memers =
                await _memerRepository.Get(
                    _ => true,
                    _ => _.Username,
                    descendingOrder: false,
                    pageIndex,
                    pageSize);

            return _mapper.Map<IPagedList<MemerViewModel>>(memers);
        }

        public async Task<Outcome<object>> IsUsernameAvailable(string? username)
        {
            var outcome = await _memerRepository.IsUsernameAvailable(username ?? string.Empty);
            return outcome.IsSuccess 
                ? Outcome<object>.FromSuccess(true, outcome.Message) 
                : Outcome<object>.FromError(outcome);
        }

        public override async Task<Outcome<MemerViewModel>> Insert(MemerInput model)
        {
            var usernameAvailableOutcome = 
                await IsUsernameAvailable(model.Username ?? string.Empty);

            if (usernameAvailableOutcome.IsError) 
                return Outcome<MemerViewModel>.FromError(usernameAvailableOutcome);

            return await base.Insert(model);
        }

        public override async Task<Outcome<MemerViewModel>> Update(MemerInput model)
        {
            var isValidId = Guid.TryParse(model.Id, out var id);
            if (!isValidId) 
                return Outcome<MemerViewModel>.FromError("Id is not a valid guid");

            var original = await Repository.GetById(id);
            if (original is null) 
                return Outcome<MemerViewModel>.FromError("Not Found.");

            if (!original.Username.InsensitiveEquals(model.Username))
            {
                var usernameAvailableOutcome =
                    await _memerRepository.IsUsernameAvailable(model.Username!);
                if (!usernameAvailableOutcome.IsError) 
                    return Outcome<MemerViewModel>.FromError(usernameAvailableOutcome);
            }

            var dataModel = Mapper.Map<Memer>(model);
            var outcome = await Repository.Update(dataModel);
            return outcome.IsSuccess
                ? Outcome<MemerViewModel>.FromSuccess(Mapper.Map<MemerViewModel>(outcome.Value))
                : Outcome<MemerViewModel>.FromError(outcome);
        }
    }
}