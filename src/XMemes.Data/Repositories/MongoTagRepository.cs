using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using XMemes.Models;
using XMemes.Models.Paging;
using Tag = XMemes.Models.Domain.Tag;

namespace XMemes.Data.Repositories
{
    public class MongoTagRepository : MongoRepository<Tag>
    {
        public MongoTagRepository(
            ILogger<MongoRepository<Tag>> logger,
            IConfiguration config,
            IOptions<Settings> settings,
            IMongoClient client)
            : base(logger, config, settings, client) { }

        public override async Task<IPagedList<Tag>> Search(
            string keyword, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var regexFilter = new Regex(keyword, RegexOptions.IgnoreCase);
            var bsonRegex = new BsonRegularExpression(regexFilter);

            var nameFilter = Builders<Tag>.Filter.Regex(_ => _.Name, bsonRegex);
            var tagsFind = Tags.Find(nameFilter);

            return await tagsFind.ToPagedList(pageIndex, pageSize);
        }
    }
}
