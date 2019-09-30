using System;

namespace ITG.Brix.Users.Domain.Bases
{
    public abstract class Entity : BaseEntity
    {
        public Guid Id { get; protected set; }
    }
}
