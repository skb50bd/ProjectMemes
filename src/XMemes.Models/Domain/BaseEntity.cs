using System;

namespace XMemes.Models.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public class TrackedEntity : BaseEntity
    {
        public bool Deleted { get; set; }
    }
}