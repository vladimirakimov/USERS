using System;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Services
{
    public interface IPublishIntegrationEventsService
    {
        Task PublishUserCreated(Guid id, string login, string firstName, string lastName);
        Task PublishUserUpdated(Guid id, string login, string firstName, string lastName);
        Task PublishUserDeleted(Guid id);
    }
}
