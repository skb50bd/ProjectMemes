using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using XMemes.Models;
using XMemes.Models.Domain;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public class MongoTemplateRepository : MongoRepository<Template>
    {
        public MongoTemplateRepository(
            ILogger<MongoRepository<Template>> logger,
            IConfiguration config,
            IOptions<Settings> settings,
            IMongoClient client)
            : base(logger, config, settings, client) { }

        public override async Task<IPagedList<Template>> Search(
            string keyword,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var regexFilter = new Regex(keyword, RegexOptions.IgnoreCase);
            var bsonRegex = new BsonRegularExpression(regexFilter);

            var nameFilter = Builders<Template>.Filter.Regex(_ => _.Name, bsonRegex);
            var templatesFind = Templates.Find(nameFilter);

            return await templatesFind.ToPagedList(pageIndex, pageSize);
        }
    }
}