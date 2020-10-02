using System.Threading.Tasks;
using XMemes.Models.InputModels;
using XMemes.Models.ViewModels;

namespace XMemes.Services.Abstractions
{
    public interface IMemerService : IService<MemerViewModel, MemerInput>
    {
        Task<bool> IsUsernameAvailable(string? username);
    }
}