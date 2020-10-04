using System;
using System.Threading.Tasks;

using XMemes.Models.Domain;
using XMemes.Models.Operations;

namespace XMemes.Data.Repositories
{
    public interface IMemeRepository : IRepository<Meme>
    {
        Task<Outcome<object>> ToggleLike(Guid memeId, Guid likerId);
    }
}