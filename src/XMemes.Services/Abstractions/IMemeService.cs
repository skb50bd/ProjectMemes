using System;
using System.Threading.Tasks;
using XMemes.Models.InputModels;
using XMemes.Models.Operations;
using XMemes.Models.ViewModels;

namespace XMemes.Services.Abstractions
{
    public interface IMemeService : IService<MemeViewModel, MemeInput>
    {
        Task<Outcome<MemeViewModel>> ToggleLike(Guid memeId, Guid likerId);
    }
}