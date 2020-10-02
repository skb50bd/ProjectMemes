using System;
using System.Threading.Tasks;

using XMemes.Models.Domain;

namespace XMemes.Data.Repositories
{
    public interface IMemeRepository : IRepository<Meme>
    {
        Task<bool> ToggleLike(Guid memeId, Guid likerId);
    }
}