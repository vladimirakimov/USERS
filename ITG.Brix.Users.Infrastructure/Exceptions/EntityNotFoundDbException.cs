using System;
using System.Runtime.Serialization;

namespace ITG.Brix.Users.Infrastructure.Exceptions
{
    [Serializable]
    public class EntityNotFoundDbException : Exception
    {
        public EntityNotFoundDbException() : base("EntityNotFound") { }

        protected EntityNotFoundDbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
