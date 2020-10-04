using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemersController : XMemesControllerBase<MemerViewModel, MemerInput>
    {
        private readonly IMemerService _memerService;

        public MemersController(
            ILogger<MemersController> logger,
            IMemerService service)
            : base(logger, service)
        {
            _memerService = service;
        }

        [HttpGet("IsUsernameAvailable/{username}")]
        public async Task<ActionResult<bool>> IsUsernameAvailable(string? username)
        {
            var outcome = await _memerService.IsUsernameAvailable(username);
            if (outcome.IsSuccess) return (bool) outcome.Value!;
            else return BadRequest(outcome.Message);
        }
    }
}
