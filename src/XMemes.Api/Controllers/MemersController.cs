using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XMemes.Models.InputModels;
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
        public async Task<bool> IsUsernameAvailable(string? username) =>
            await _memerService.IsUsernameAvailable(username);
    }
}