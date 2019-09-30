using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Internal;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Application.Services;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Providers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Cqs.Commands.Handlers
{
    public class CreateCommandHandler : IRequestHandler<CreateCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly IVersionProvider _versionProvider;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IPublishIntegrationEventsService _publishIntegrationEventsService;

        public CreateCommandHandler(IUserRepository userRepository,
                                    IIdentifierProvider identifierProvider,
                                    IVersionProvider versionProvider,
                                    IPasswordProvider passwordProvider,
                                    IPublishIntegrationEventsService publishIntegrationEventsService)
        {
            _userRepository = userRepository ?? throw Error.ArgumentNull(nameof(userRepository));
            _identifierProvider = identifierProvider ?? throw Error.ArgumentNull(nameof(identifierProvider));
            _versionProvider = versionProvider ?? throw Error.ArgumentNull(nameof(versionProvider));
            _passwordProvider = passwordProvider ?? throw Error.ArgumentNull(nameof(passwordProvider));
            _publishIntegrationEventsService = publishIntegrationEventsService;
        }

        public async Task<Result> Handle(CreateCommand command, CancellationToken cancellationToken)
        {
            var id = _identifierProvider.Generate();
            var login = new Login(command.Login);
            var passwordHashed = _passwordProvider.Hash(command.Password);

            var firstName = new FirstName(command.FirstName);
            var lastName = new LastName(command.LastName);
            var fullName = new FullName(firstName, lastName);
            var userToCreate = new User(id, login, passwordHashed, fullName);

            userToCreate.Version = _versionProvider.Generate();
            Result result;

            try
            {
                await _userRepository.Create(userToCreate);
                result = Result.Ok(id, userToCreate.Version);
                await _publishIntegrationEventsService.PublishUserCreated(id, command.Login, command.FirstName, command.LastName);
            }
            catch (UniqueKeyException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.Conflict.Name,
                                            Message = HandlerFailures.Conflict,
                                            Target = "login"}
                                        }
                );
            }
            catch
            {
                result = Result.Fail(CustomFailures.CreateUserFailure);
            }

            return result;
        }
    }
}
