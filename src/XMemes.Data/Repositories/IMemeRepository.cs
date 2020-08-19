using LiteDB;

using XMemes.Models.Domain;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public interface IMemeRepository : IRepository<Meme>
    {
        Meme? GetById(ObjectId id, bool includeAll = false);
        Meme? GetLatest();
        int LikeMeme(ObjectId memeId, ObjectId memerId);
        IPagedList<Meme> GetPopularMemes(int pageIndex, int pageSize);
    }
}