using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using XMemes.Models;
using XMemes.Models.Domain;
using XMemes.Models.Paging;

using Tag = XMemes.Models.Domain.Tag;

namespace XMemes.Data.Repositories
{
    public abstract class MongoRepository<T> : IRepository<T> where T : TrackedEntity
    {
        protected readonly Settings Settings;
        protected readonly ILogger<MongoRepository<T>> Logger;
        protected readonly IMongoClient Client;
        protected readonly IMongoDatabase Database;

        protected IMongoCollection<Meme> Memes =>
            Database.GetCollection<Meme>(nameof(Meme));

        protected IMongoCollection<Memer> Memers =>
            Database.GetCollection<Memer>(nameof(Memer));

        protected IMongoCollection<Tag> Tags =>
            Database.GetCollection<Tag>(nameof(Tag));

        protected IMongoCollection<Template> Templates =>
            Database.GetCollection<Template>(nameof(Template));

        private IMongoCollection<T> Collection =>
            Database.GetCollection<T>(typeof(T).Name);

        protected MongoRepository(
            ILogger<MongoRepository<T>> logger,
            IConfiguration config,
            IOptions<Settings> settings,
            IMongoClient client)
        {
            Logger = logger;
            Settings = settings.Value;

            Client = client;
            Database = Client.GetDatabase("xmemes");
            EnsureIndices().Wait();
        }

        private async Task EnsureIndices()
        {
            var indexOptions = new CreateIndexOptions();

            var primaryStringCollation = new Collation("en", strength: CollationStrength.Primary);
            var stringIndexOptions = new CreateIndexOptions { Collation = primaryStringCollation};
            var uniqueStringIndexOptions = new CreateIndexOptions { Unique = true, Collation = primaryStringCollation};

            var memeNameIndexKey = Builders<Meme>.IndexKeys.Ascending(_ => _.Name);
            var memeNameIndex = new CreateIndexModel<Meme>(memeNameIndexKey, stringIndexOptions);
            //var memeLikesIndexKey = Builders<Meme>.IndexKeys.Descending(_ => _.LikerIds);
            //var memeLikesIndex = new CreateIndexModel<Meme>(memeLikesIndexKey, indexOptions);
            var memeIfOriginalIndexKey = Builders<Meme>.IndexKeys.Ascending(_ => _.Original);
            var memeIfOriginalIndex = new CreateIndexModel<Meme>(memeIfOriginalIndexKey, indexOptions);
            var memeIfNsfwIndexKey = Builders<Meme>.IndexKeys.Ascending(_ => _.Nsfw);
            var memeIfNsfwIndex = new CreateIndexModel<Meme>(memeIfNsfwIndexKey, indexOptions);
            var memeSubmissionDateIndexKey = Builders<Meme>.IndexKeys.Ascending(_ => _.SubmittedAt);
            var memeSubmissionDateIndex = new CreateIndexModel<Meme>(memeSubmissionDateIndexKey, indexOptions);
            await Memes.Indexes.CreateManyAsync(
                new[]
                {
                    memeNameIndex,
                    //memeLikesIndex,
                    memeIfOriginalIndex,
                    memeIfNsfwIndex,
                    memeSubmissionDateIndex
                });

            var memerNameIndexKey = Builders<Memer>.IndexKeys.Ascending(_ => _.Name);
            var memerNameIndex = new CreateIndexModel<Memer>(memerNameIndexKey, stringIndexOptions);
            var memerUsernameIndexKey = Builders<Memer>.IndexKeys.Ascending(_ => _.Username);
            var memerUsernameIndex = new CreateIndexModel<Memer>(memerUsernameIndexKey, uniqueStringIndexOptions);
            //var memerLikesIndexKey = Builders<Memer>.IndexKeys.Descending(_ => _.TotalLikes);
            //var memerLikesIndex = new CreateIndexModel<Memer>(memerLikesIndexKey, indexOptions);
            await Memers.Indexes.CreateManyAsync(
                new[]
                {
                    memerNameIndex,
                    memerUsernameIndex
                    //memerLikesIndex
                });

            var tagNameIndexKey = Builders<Tag>.IndexKeys.Ascending(_ => _.Name);
            var tagNameIndex = new CreateIndexModel<Tag>(tagNameIndexKey, stringIndexOptions);
            await Tags.Indexes.CreateOneAsync(tagNameIndex);


            var templateNameIndexKey = Builders<Template>.IndexKeys.Ascending(_ => _.Name);
            var templateNameIndex = new CreateIndexModel<Template>(templateNameIndexKey, stringIndexOptions);
            var templateIfNsfwIndexKey = Builders<Template>.IndexKeys.Ascending(_ => _.Nsfw);
            var templateIfNsfwIndex = new CreateIndexModel<Template>(templateIfNsfwIndexKey, indexOptions);
            var templateSubmissionDateIndexKey = Builders<Template>.IndexKeys.Ascending(_ => _.SubmittedAt);
            var templateSubmissionDateIndex = new CreateIndexModel<Template>(templateSubmissionDateIndexKey, indexOptions);
            await Templates.Indexes.CreateManyAsync(
                new[]
                {
                   templateNameIndex,
                   templateIfNsfwIndex,
                   templateSubmissionDateIndex
                });
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            var filter = Builders<T>.Filter.Where(i => i.Id == id && !i.Deleted);

            var cursor = await Collection.FindAsync(filter);
            var item = await cursor.FirstOrDefaultAsync();
            return item;
        }

        public virtual async Task<IList<T>> GetAll()
        {
            var filter = Builders<T>.Filter.Where(i => !i.Deleted);

            var cursor = await Collection.FindAsync(filter);
            var items = await cursor.ToListAsync();

            return items;
        }

        public virtual async Task<IPagedList<T>> Get<TKey>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy,
            bool descendingOrder,
            int pageIndex,
            int pageSize)
        {
            var filtered =
                Collection.AsQueryable().Where(predicate);

            var ordered =
                descendingOrder
                    ? filtered.OrderByDescending(orderBy)
                    : filtered.OrderBy(orderBy);

            return await ordered.ToPagedList(pageIndex, pageSize);
        }

        public virtual async Task<bool> Insert(T item)
        {
            try
            {
                await Collection.InsertOneAsync(item);
            }
            catch (Exception e)
            {
                var json = item.ToJson();
                Logger.LogError("Error inserting document.\n Item: {0}. \n{1}",
                    json,
                    e.Message);
                return false;
            }

            return true;
        }


        public virtual System.Linq.IQueryable<T> GetQueryable() =>
            Collection.AsQueryable();

        public virtual async Task<bool> Update(T item)
        {
            try
            {
                var result =
                    await Collection.ReplaceOneAsync(i => i.Id == item.Id, item);

                return result.IsAcknowledged && result.IsModifiedCountAvailable;
            }
            catch (Exception e)
            {
                var json = item.ToJson();
                Logger.LogError("Error updating document. \n{0}\n{1}", json, e.Message);
                return false;
            }
        }

        public virtual async Task<bool> Delete(T item)
        {
            try
            {
                var result =
                    await Collection.DeleteOneAsync(i => i.Id == item.Id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception e)
            {
                Logger.LogError(
                    "Error deleting document. \n{0}\n{1}",
                    item.ToJson(),
                    e.Message);
                return false;
            }
        }

        public abstract Task<IPagedList<T>> Search(string keyword, int pageIndex = 0, int pageSize = 20);

        public async Task<bool> Exists(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(_ => _.Id, id);
            var count = await Collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public void Dispose() { }
    }
}
