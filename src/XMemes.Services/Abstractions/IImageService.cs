using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XMemes.Services.Abstractions
{
    public interface IImageService
    {
        Task<string?> GetUrl(string filename);
        
        Task<string?> Upload(string filename, Stream fileStream);

        Task<string?> Upload(string filename, byte[] bytes);

        Task<string?> Upload(string filename, string uploadFilePath);

        Task<FileInfo?> Download(string filename);

        Task<bool> Exists(string filename);

        Task<IList<string>> GetAllFilenames();

        Task<bool> Delete(string filename);
    }
}