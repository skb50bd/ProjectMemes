using System.Threading.Tasks;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.ViewModels;

namespace XMemes.Services.Abstractions
{
    public interface IMemerService : IService<MemerViewModel, MemerInput>
    {
        Task<Outcome<object>> IsUsernameAvailable(string? username);
    }
}