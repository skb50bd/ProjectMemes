using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XMemes.Services.Abstractions;

namespace XMemes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService; 
            
        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetById(string id)
        {
            var url = await _imageService.GetUrl(id);
            if (url is null) return NotFound();
            return url;
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> Post(IFormFile image)
        {
            if (image.Length == 0) return BadRequest();
            
            var extension = Path.GetExtension(image.FileName);
            var uniqueFilename = Guid.NewGuid() + extension;
            
            await using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            var filename = await _imageService.Upload(uniqueFilename, stream);
            stream.Close();

            if (filename is null) return BadRequest("Could not upload the file");
            return Ok(filename);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _imageService.Delete(id);
            if (success) return Ok();
            return BadRequest();
        }
    }
}