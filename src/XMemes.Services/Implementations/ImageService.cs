using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XMemes.Data.Repositories;
using XMemes.Services.Abstractions;

namespace XMemes.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IFileRepository _fileRepository;

        public ImageService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Task<string?> GetUrl(string filename) =>
            _fileRepository.GetUrl(filename);

        public async Task<string?> Upload(string filename, Stream fileStream) => 
            await _fileRepository.Upload(filename, fileStream);

        public async Task<string?> Upload(string filename, byte[] bytes) =>
            await _fileRepository.Upload(filename, bytes);

        public async Task<string?> Upload(string filename, string uploadFilePath) =>
            await _fileRepository.Upload(filename, uploadFilePath);

        public async Task<FileInfo?> Download(string filename) =>
            await _fileRepository.Download(filename);

        public async Task<bool> Exists(string filename) =>
            await _fileRepository.Exists(filename);

        public async Task<IList<string>> GetAllFilenames() =>
            await _fileRepository.GetAllFilenames();

        public async Task<bool> Delete(string filename) =>
            await _fileRepository.Delete(filename);
    }
}
