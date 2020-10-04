using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

using XMemes.Models.InputModels;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemesController : XMemesControllerBase<MemeViewModel, MemeInput>
    {
        private readonly IMemeService _memeService;
        public MemesController(
            ILogger<MemesController> logger,
            IMemeService service, IMemeService memeService)
            : base(logger, service)
        {
            _memeService = memeService;
        }

        [HttpPost("Like")]
        public async Task<ActionResult<MemeViewModel>> ToggleLike(string memeId, string likerId)
        {
            var memeIdIsValid = Guid.TryParse(memeId, out var memeIdGuid);
            var likerIdIsValid = Guid.TryParse(likerId, out var likerIdGuid);

            if (!memeIdIsValid || !likerIdIsValid) return NotFound();

            var outcome = await _memeService.ToggleLike(memeIdGuid, likerIdGuid);
            if (outcome.IsSuccess) return outcome.Value!;
            else return BadRequest(outcome.Message);
        }
    }
}
