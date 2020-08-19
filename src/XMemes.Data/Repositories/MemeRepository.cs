using LiteDB;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.Linq;
using Microsoft.Extensions.Logging;
using XMemes.Models;
using XMemes.Models.Domain;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public class MemeRepository : Repository<Meme>, IMemeRepository
    {
        public MemeRepository(
            ILogger<MemeRepository> logger,
            IConfiguration config,
            IOptionsMonitor<Settings> settingsMonitor)
        : base(logger, config, settingsMonitor) { }

        public Meme? GetById(ObjectId id, bool includeAll = false)
        {
            var meme =
                includeAll
                    ? Memes
                        .Include(m => m.Memer)
                        .Include(m => m.Likers)
                        .Include(m => m.Tags)
                        .Include(m => m.Template)
                        .FindById(id)
                    : base.GetById(id);

            return meme;
        }

        public Meme? GetLatest()
        {
            var latestMeme =
                Memes.FindOne(Query.All(Query.Descending));
            return latestMeme;
        }

        public int LikeMeme(ObjectId memeId, ObjectId memerId)
        {
            var meme = Memes.Include(m => m.Likers).FindById(memeId);
            var memer = Memers.FindById(memerId);

            if (meme is null || memer is null)
                return -1;

            if (meme.Likers.All(l => l.Id != memerId))
            {
                meme.Likers.Add(memer);
            }
            return meme.Likers.Count;
        }

        public IPagedList<Meme> GetPopularMemes(int pageIndex = 0, int pageSize = -1)
        {
            if (pageSize == -1)
                pageSize = Settings.PageSize;

            var query =
                Query.GTE("$COUNT($.Likers[*])", Settings.PopularityThreshHold);

            var memes = Repo.Query<Meme>(query);

            return memes.ToPagedList(pageIndex, pageSize);
        }
    }
}