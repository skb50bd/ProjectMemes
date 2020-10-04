using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XMemes.Models.Operations;

namespace XMemes.Services.Abstractions
{
    public interface IImageService
    {
        Task<Outcome<string>> GetUrl(string filename);
        
        Task<Outcome<string>> Upload(string filename, Stream fileStream);

        Task<Outcome<string>> Upload(string filename, byte[] bytes);

        Task<Outcome<string>> Upload(string filename, string uploadFilePath);

        Task<FileInfo?> Download(string filename);

        Task<bool> Exists(string filename);

        Task<IList<string>> GetAllFilenames();

        Task<Outcome<object>> Delete(string filename);
    }
}