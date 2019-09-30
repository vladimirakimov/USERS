using System;
using System.Runtime.Serialization;

namespace ITG.Brix.Users.Infrastructure.Exceptions
{
    [Serializable]
    public class UniqueKeyException : Exception
    {
        public UniqueKeyException() : base("UniqueKey") { }

        public UniqueKeyException(Exception ex) : base(ex.Message, ex) { }

        protected UniqueKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
