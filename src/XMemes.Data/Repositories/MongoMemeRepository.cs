using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using XMemes.Models;
using XMemes.Models.Domain;
using XMemes.Models.Operations;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public class MongoMemeRepository : MongoRepository<Meme>, IMemeRepository
    {
        public MongoMemeRepository(
            ILogger<MongoRepository<Meme>> logger,
            IConfiguration config,
            IOptions<Settings> settings,
            IMongoClient client)
        : base(logger, config, settings, client) { }

        public async Task<Outcome<object>> ToggleLike(Guid memeId, Guid likerId)
        {
            try
            {
                using var session = await Client.StartSessionAsync();

                var memeFilter = Builders<Meme>.Filter.Where(_ => _.Id == memeId);

                //var memerId =
                //    await Memes.AsQueryable()
                //        .Where(_ => _.Id == memeId)
                //        .Select(_ => _.MemerId)
                //        .FirstOrDefaultAsync();

                var isLikedFilter =
                    Builders<Meme>.Filter.And(
                        memeFilter,
                        Builders<Meme>.Filter.AnyEq(_ => _.LikerIds, likerId));

                var unlikeMemeUpdate =
                    Builders<Meme>.Update.Pull(_ => _.LikerIds, likerId);

                //var unlikeMemerUpdate =
                //    Builders<Memer>.Update.Inc(_ => _.TotalLikes, -1);

                var unlikeMemeResult = await Memes.UpdateOneAsync(session, isLikedFilter, unlikeMemeUpdate);
                //var unlikeMemerResult = await Memers.UpdateOneAsync(session, _ => _.Id == memerId, unlikeMemerUpdate);

                var isNotLikedFilter =
                    Builders<Meme>.Filter.And(
                        memeFilter,
                        Builders<Meme>.Filter.AnyNe(_ => _.LikerIds, likerId));

                var likeMemeUpdate =
                    Builders<Meme>.Update.Push(_ => _.LikerIds, likerId);

                //var likeMemerUpdate =
                //    Builders<Memer>.Update.Inc(_ => _.TotalLikes, 1);

                var likeMemeResult = await Memes.UpdateOneAsync(session, isNotLikedFilter, likeMemeUpdate);
                //var likeMemerResult = await Memers.UpdateOneAsync(session, _ => _.Id == memerId, likeMemerUpdate);

                await session.CommitTransactionAsync();

                return
                    unlikeMemeResult.IsAcknowledged
                    && likeMemeResult.IsAcknowledged
                    //&& likeMemerResult.IsAcknowledged
                    && unlikeMemeResult.ModifiedCount
                    + likeMemeResult.ModifiedCount > 1
                        //+ unlikeMemerResult.ModifiedCount
                        //+ likeMemerResult.ModifiedCount == 2;
                        ? Outcome<object>.FromSuccess(true)
                        : throw new Exception($"Toggling like on meme: {memeId} failed for liker: {likerId}");
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected error occurred updating like count.", e);
                return Outcome<object>.FromError("Error toggling like.", e);
            }
        }

        public override async Task<IPagedList<Meme>> Search(string keyword, int pageIndex = 0, int pageSize = 20)
        {
            var regexFilter = new Regex(keyword, RegexOptions.IgnoreCase);
            var bsonRegex = new BsonRegularExpression(regexFilter);

            var nameFilter = Builders<Meme>.Filter.Regex(_ => _.Name, bsonRegex);
            var descriptionFilter = Builders<Meme>.Filter.Regex(_ => _.Description, bsonRegex);
            var filter = Builders<Meme>.Filter.And(nameFilter, descriptionFilter);
            var memesFind = Memes.Find(filter);

            return await memesFind.ToPagedList(pageIndex, pageSize);
        }
    }
}