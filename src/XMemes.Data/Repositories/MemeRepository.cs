using LiteDB;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;

using XMemes.Models;
using XMemes.Models.Domain;

namespace XMemes.Data.Repositories
{
    public class MemeRepository : Repository<Meme>, IMemeRepository
    {
        public MemeRepository(
            IConfiguration config,
            IOptionsMonitor<BanglaMemesOptions> opts)
        : base(config, opts) { }

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

        public IList<Meme> GetPopularMemes(int start = 0, int count = -1)
        {
            if (count == -1)
                count = _opts.PopularityThreshHold;

            var query =
                Query.GTE("$COUNT($.Likers[*])", _opts.PopularityThreshHold);

            var memes =
                _repo.Query<Meme>(query)
                    .Skip(start)
                    .Limit(count);

            return memes?.ToList() ?? new List<Meme>();
        }
    }
}