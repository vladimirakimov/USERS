using ITG.Brix.Communication.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Services.Impl
{
    public class PublishIntegrationEventsService : IPublishIntegrationEventsService
    {
        private readonly IMessageSession _endpoint;

        public PublishIntegrationEventsService(IMessageSession endpoint)
        {
            _endpoint = endpoint;

        }

        public async Task PublishUserCreated(Guid id, string login, string firstName, string lastName)
        {
            var options = new PublishOptions();
            var @event = new UserCreated
            {
                Id = id,
                Login = login,
                FirstName = firstName,
                LastName = lastName
            };


            options.SetMessageId(id.ToString());

            await _endpoint.Publish(@event, options);
        }

        public async Task PublishUserDeleted(Guid id)
        {
            var options = new PublishOptions();
            var @event = new UserDeleted
            {
                Id = id
            };

            options.SetMessageId(id.ToString());

            await _endpoint.Publish(@event, options);
        }

        public async Task PublishUserUpdated(Guid id, string login, string firstName, string lastName)
        {
            var options = new PublishOptions();
            var @event = new UserUpdated
            {
                Id = id,
                Login = login,
                FirstName = firstName,
                LastName = lastName
            };

            options.SetMessageId(id.ToString());

            await _endpoint.Publish(@event, options);
        }
    }
}
