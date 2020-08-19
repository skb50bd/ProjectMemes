using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;

using XMemes.Models.Domain;

namespace XMemes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemesController : ControllerBase
    {
        private readonly ILogger<MemesController> _logger;

        public MemesController(ILogger<MemesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Meme> Get()
        {
            return new[]
            {
                new Meme
                {
                    Name = "Latest Jinish",
                    SubmittedAt = DateTimeOffset.UtcNow
                },
                new Meme
                {
                    Name = "Tor ki dor lagbo?",
                    SubmittedAt = DateTimeOffset.UtcNow.AddDays(-1.4)
                }
            };
        }
    }
}
