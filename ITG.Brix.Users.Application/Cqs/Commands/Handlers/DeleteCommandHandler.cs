using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Internal;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Application.Services;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Cqs.Commands.Handlers
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishIntegrationEventsService _publishIntegrationEventsService;

        public DeleteCommandHandler(IUserRepository userRepository, IPublishIntegrationEventsService publishIntegrationEventsService)
        {
            _userRepository = userRepository ?? throw Error.ArgumentNull(nameof(userRepository));
            _publishIntegrationEventsService = publishIntegrationEventsService ?? throw Error.ArgumentNull(nameof(publishIntegrationEventsService));
        }

        public async Task<Result> Handle(DeleteCommand command, CancellationToken cancellationToken)
        {
            Result result;

            try
            {
                await _userRepository.Delete(command.Id, command.Version);
                result = Result.Ok();
                await _publishIntegrationEventsService.PublishUserDeleted(command.Id);
            }
            catch (EntityNotFoundDbException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.NotFound.Name,
                                            Message = HandlerFailures.NotFound,
                                            Target = "id"}
                                        }
                );
            }
            catch (EntityVersionDbException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.NotMet.Name,
                                            Message = HandlerFailures.NotMet,
                                            Target = "version"}
                                        }
                );
            }
            catch
            {
                result = Result.Fail(CustomFailures.DeleteUserFailure);
            }

            return result;
        }
    }
}
