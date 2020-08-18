using LiteDB;

namespace XMemes.Models.Domain {
    public abstract class BaseEntity {
        public ObjectId Id { get; set; }

        protected BaseEntity()
        {
            Id = ObjectId.NewObjectId();
        }
    }

    public abstract class TrackedEntity: BaseEntity {
        public bool Deleted { get; set; }
    }
}