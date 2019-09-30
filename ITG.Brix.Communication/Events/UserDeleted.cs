using NServiceBus;
using System;

namespace ITG.Brix.Communication.Events
{
    public class UserDeleted : IEvent
    {
        public Guid Id { get; set; }
    }
}
