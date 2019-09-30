using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Exceptions;
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
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result>
    {
        private readonly IUserFinder _userFinder;
        private readonly IUserRepository _userRepository;
        private readonly IVersionProvider _versionProvider;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IPublishIntegrationEventsService _publishIntegrationEventsService;

        public UpdateCommandHandler(IUserFinder userFinder,
                                    IUserRepository userRepository,
                                    IVersionProvider versionProvider,
                                    IPasswordProvider passwordProvider,
                                    IPublishIntegrationEventsService publishIntegrationEventsService
            )
        {
            _userFinder = userFinder ?? throw Error.ArgumentNull(nameof(userFinder));
            _userRepository = userRepository ?? throw Error.ArgumentNull(nameof(userRepository));
            _versionProvider = versionProvider ?? throw Error.ArgumentNull(nameof(versionProvider));
            _passwordProvider = passwordProvider ?? throw Error.ArgumentNull(nameof(passwordProvider));
            _publishIntegrationEventsService = publishIntegrationEventsService;
        }

        public async Task<Result> Handle(UpdateCommand command, CancellationToken cancellationToken)
        {
            Result result;
            try
            {
                var user = await _userFinder.Get(command.Id);
                if (user.Version != command.Version)
                {
                    throw new CommandVersionException();
                }
                if (command.Login.HasValue)
                {
                    var updatedLogin = command.Login.Value;
                    user.ChangeLogin(new Login(updatedLogin));
                }
                if (command.Password.HasValue)
                {
                    var passwordHashed = _passwordProvider.Hash(command.Password.Value);
                    user.ChangePassword(passwordHashed);
                }
                if (command.FirstName.HasValue)
                {
                    user.FullName.ChangeFirstName(command.FirstName.Value);
                }
                if (command.LastName.HasValue)
                {
                    user.FullName.ChangeLastName(command.LastName.Value);
                }
                user.Version = _versionProvider.Generate();
                await _userRepository.Update(user);
                result = Result.Ok(user.Version);
                await _publishIntegrationEventsService.PublishUserUpdated(user.Id, user.Login.Value, user.FullName.FirstName.Value, user.FullName.LastName.Value);
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
            catch (CommandVersionException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.NotMet.Name,
                                            Message = HandlerFailures.NotMet,
                                            Target = "version"}
                                        }
                );
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
                result = Result.Fail(CustomFailures.UpdateUserFailure);
            }

            return result;
        }
    }
}
