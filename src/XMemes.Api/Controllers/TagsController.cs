using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using XMemes.Models.InputModels;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : XMemesControllerBase<TagViewModel, TagInput>
    {
        public TagsController(
            ILogger<TagsController> logger,
            IService<TagViewModel, TagInput> service)
        : base(logger, service) { }
    }
}
