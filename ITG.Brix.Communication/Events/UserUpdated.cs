using NServiceBus;
using System;

namespace ITG.Brix.Communication.Events
{
    public class UserUpdated : IEvent
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
