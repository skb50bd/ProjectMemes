using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using XMemes.Models;
using XMemes.Models.Domain;
using XMemes.Models.Paging;
using XMemes.Models.Utils;

namespace XMemes.Data.Repositories
{
    public class MongoMemerRepository : MongoRepository<Memer> , IMemerRepository
    {
        public MongoMemerRepository(
            ILogger<MongoRepository<Memer>> logger,
            IConfiguration config,
            IOptions<Settings> settings,
            IMongoClient client)
            : base(logger, config, settings, client) { }

        public override async Task<IPagedList<Memer>> Search(
            string keyword,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var regexFilter = new Regex(keyword, RegexOptions.IgnoreCase);
            var bsonRegex = new BsonRegularExpression(regexFilter);

            var nameFilter = Builders<Memer>.Filter.Regex(_ => _.Name, bsonRegex);
            var usernameFilter = Builders<Memer>.Filter.Regex(_ => _.Username, bsonRegex);
            var filter = Builders<Memer>.Filter.And(nameFilter, usernameFilter);
            var memersFind = Memers.Find(filter);

            return await memersFind.ToPagedList(pageIndex, pageSize);
        }

        public async Task<bool> IsUsernameAvailable(string username)
        {
            username = $@"\b{username}\b";
            var regexFilter = new Regex(username, RegexOptions.IgnoreCase);
            var bsonRegex = new BsonRegularExpression(regexFilter);

            var usernameFilter = Builders<Memer>.Filter.Regex(_ => _.Username, bsonRegex);
            var count = await Memers.CountDocumentsAsync(usernameFilter);
            return count == 0;
        }
    }
}