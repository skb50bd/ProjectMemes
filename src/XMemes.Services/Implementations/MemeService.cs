using AutoMapper;

using System;
using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.Paging;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Services.Implementations
{
    public class MemeService : ServiceBase<Meme, MemeViewModel, MemeInput>, IMemeService
    {
        private IMemeRepository _memeRepository;
        public MemeService(
            IMemeRepository memeRepository,
            IMapper mapper)
        : base(memeRepository, mapper)
        {
            _memeRepository = memeRepository;
        }

        public override async Task<IPagedList<MemeViewModel>> GetAll(
           int pageIndex = 0,
           int pageSize = 20)
        {
            var memes =
                await Repository.Get(
                    _ => true,
                    _ => _.SubmittedAt,
                    descendingOrder: true,
                    pageIndex,
                    pageSize);

            return Mapper.Map<IPagedList<MemeViewModel>>(memes);
        }

        public async Task<Outcome<MemeViewModel>> ToggleLike(Guid memeId, Guid likerId)
        {
            var outcome = await _memeRepository.ToggleLike(memeId, likerId);
            if (outcome.IsSuccess)
            {
                var memeData = await _memeRepository.GetById(memeId);
                var memeVm = Mapper.Map<MemeViewModel>(memeData);
                return Outcome<MemeViewModel>.FromSuccess(memeVm, outcome.Message);
            }
            return Outcome<MemeViewModel>.FromError(outcome);
        }
    }
}
