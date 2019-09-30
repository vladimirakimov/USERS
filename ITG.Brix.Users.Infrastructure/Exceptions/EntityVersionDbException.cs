using System;
using System.Runtime.Serialization;

namespace ITG.Brix.Users.Infrastructure.Exceptions
{
    [Serializable]
    public class EntityVersionDbException : Exception
    {
        public EntityVersionDbException() : base("EntityVersion") { }

        protected EntityVersionDbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
