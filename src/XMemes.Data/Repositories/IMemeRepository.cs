using System.Collections.Generic;
using LiteDB;
using XMemes.Models.Domain;

namespace XMemes.Data.Repositories
{
    public interface IMemeRepository : IRepository<Meme>
    {
        Meme? GetById(ObjectId id, bool includeAll = false);
        Meme? GetLatest();
        int LikeMeme(ObjectId memeId, ObjectId memerId);
        IList<Meme> GetPopularMemes(int start = 0, int count = -1);
    }
}