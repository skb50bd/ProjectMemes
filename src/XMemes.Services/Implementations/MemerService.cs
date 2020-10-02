using AutoMapper;

using System;
using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using XMemes.Models.InputModels;
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

        public async Task<bool> IsUsernameAvailable(string? username) =>
            await _memerRepository.IsUsernameAvailable(username ?? string.Empty);

        public override async Task<bool> Insert(MemerInput model)
        {
            var usernameAvailable = await _memerRepository.IsUsernameAvailable(model.Username ?? string.Empty);
            if (!usernameAvailable) return false;

            var dataModel = Mapper.Map<Memer>(model);
            return await Repository.Insert(dataModel);
        }

        public override async Task<bool> Update(MemerInput model)
        {
            var isValidId = Guid.TryParse(model.Id, out var id);
            if (!isValidId) return false;

            var original = await Repository.GetById(id);
            if (original is null) return false;

            if (!original.Username.InsensitiveEquals(model.Username))
            {
                var usernameAvailable =
                    await _memerRepository.IsUsernameAvailable(model.Username!);
                if (!usernameAvailable) return false;
            }

            var dataModel = Mapper.Map<Memer>(model);
            return await Repository.Update(dataModel);
        }
    }
}