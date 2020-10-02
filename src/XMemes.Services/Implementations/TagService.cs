using AutoMapper;

using System.Threading.Tasks;

using XMemes.Data.Repositories;
using XMemes.Models.InputModels;
using XMemes.Models.Paging;
using XMemes.Models.ViewModels;

using Tag = XMemes.Models.Domain.Tag;

namespace XMemes.Services.Implementations
{
    public class TagService : ServiceBase<Tag, TagViewModel, TagInput>
    {
        private readonly IRepository<Tag> _tagRepository;
        private readonly IMapper _mapper;

        public TagService(
            IRepository<Tag> tagRepository,
            IMapper mapper)
        : base(tagRepository, mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public override async Task<IPagedList<TagViewModel>> GetAll(
            int pageIndex = 0,
            int pageSize = 20)
        {
            var tags =
                await _tagRepository.Get(
                    t => true,
                    t => t.Name,
                    descendingOrder: false,
                    pageIndex,
                    pageSize);

            return _mapper.Map<IPagedList<TagViewModel>>(tags);
        }
    }
}