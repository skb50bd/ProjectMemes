using System;
using LiteDB;
using XMemes.Models;
using XMemes.Models.Domain;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XMemes.Models.Paging;

namespace XMemes.Data.Repositories
{
    public class Repository<T>: IRepository<T> where T: BaseEntity {
        protected readonly ILiteRepository Repo;
        protected readonly Settings Settings;
        protected readonly ILogger<Repository<T>> Logger;

        protected ILiteCollection<Meme> Memes =>
            Repo.Database.GetCollection<Meme>();

        protected ILiteCollection<Memer> Memers =>
            Repo.Database.GetCollection<Memer>();

        protected ILiteCollection<Tag> Tags =>
            Repo.Database.GetCollection<Tag>();

        protected ILiteCollection<Template> Templates =>
            Repo.Database.GetCollection<Template>();

        public Repository(
            ILogger<Repository<T>> logger,
            IConfiguration config, 
            IOptionsMonitor<Settings> settingsMonitor)
        {
            Logger = logger;
            Settings = settingsMonitor.CurrentValue;
            
            var mapper = MapEntities(BsonMapper.Global);

            Repo = new LiteRepository(
                config.GetConnectionString("LiteDB"),
                mapper);

            EnsureIndices();
        }

        private void EnsureIndices()
        {
            Repo.EnsureIndex<Meme>(nameof(Meme.Name), $"LOWER($.{nameof(Meme.Name)})");
            Memes.EnsureIndex(m => m.Hash);
            Memes.EnsureIndex(m => m.Original);
            Memes.EnsureIndex(m => m.Nsfw);
            Memes.EnsureIndex(m => m.SubmittedAt);

            Memers.EnsureIndex(nameof(Memer.Name), $"LOWER($.{nameof(Memer.Name)})");
            Memers.EnsureIndex(nameof(Memer.Username), $"LOWER($.{nameof(Memer.Username)})", true);
            Memers.EnsureIndex(m => m.TotalLikes);

            Tags.EnsureIndex(nameof(Tag.Name), $"LOWER($.{nameof(Tag.Name)})");

            Templates.EnsureIndex(nameof(Template.Name), $"LOWER($.{nameof(Template.Name)})");
            Templates.EnsureIndex(t => t.SubmittedAt);
            Templates.EnsureIndex(t => t.Nsfw);
        }

        private BsonMapper MapEntities(BsonMapper mapper)
        {
            mapper.Entity<Meme>()
                .DbRef(m => m.Memer)
                .DbRef(m => m.Template)
                .DbRef(m => m.Tags)
                .DbRef(m => m.Likers);

            mapper.Entity<Memer>()
                .DbRef(m => m.Memes);

            mapper.Entity<Tag>()
                .DbRef(t => t.Memes);

            mapper.Entity<Template>()
                .DbRef(t => t.Memer)
                .DbRef(t => t.Memes);

            return mapper;
        }

        public virtual T? GetById(ObjectId id) {
            var item = Repo.Database.GetCollection<T>().FindById(id);
            return item;
        }

        public virtual IList<T> GetAll() => 
            Repo.Database
                .GetCollection<T>()
                .FindAll()
                .ToList();

        public virtual IPagedList<T> Get(
            Expression<Func<T, bool>> predicate, 
            int pageIndex, 
            int pageSize) => 
            Repo.Query<T>()
                .Where(predicate)
                .ToPagedList(pageIndex, pageSize);

        public virtual bool Insert(T item)
        {
            var result = Repo.Database.GetCollection<T>().Insert(item);
            return !(result is null);
        }
        
        public virtual bool Update(T item) =>
            Repo.Database.GetCollection<T>().Update(item);
            
        public virtual bool Delete(T item) =>
            Repo.Database.GetCollection<T>().Delete(item.Id);

        public void Dispose() => Repo.Dispose();
    }
}