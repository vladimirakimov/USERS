using System;
using System.Runtime.Serialization;

namespace ITG.Brix.Users.Application.Exceptions
{
    [Serializable]
    public class CommandVersionException : Exception
    {
        public CommandVersionException() : base("CommandVersion") { }

        protected CommandVersionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
