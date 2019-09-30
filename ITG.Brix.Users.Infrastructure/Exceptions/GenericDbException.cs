using System;
using System.Runtime.Serialization;

namespace ITG.Brix.Users.Infrastructure.Exceptions
{
    [Serializable]
    public class GenericDbException : Exception
    {
        public GenericDbException(Exception ex) : base("Generic", ex) { }

        protected GenericDbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
