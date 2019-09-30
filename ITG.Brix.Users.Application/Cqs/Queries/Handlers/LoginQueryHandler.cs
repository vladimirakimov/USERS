using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Internal;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Providers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Cqs.Queries.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result>
    {
        private readonly IUserFinder _userFinder;
        private readonly IPasswordProvider _passwordProvider;

        public LoginQueryHandler(IUserFinder userFinder,
                                 IPasswordProvider passwordProvider)
        {
            _userFinder = userFinder ?? throw Error.ArgumentNull(nameof(userFinder));
            _passwordProvider = passwordProvider ?? throw Error.ArgumentNull(nameof(passwordProvider));
        }

        public async Task<Result> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            Result result;

            var passwordHash = _passwordProvider.Hash(request.Password);

            try
            {
                var user = await _userFinder.Get(request.Login);
                var passwordMatch = _passwordProvider.Verify(request.Password, user.Password);
                if (passwordMatch)
                {
                    result = Result.Ok(new AuthenticatedUserModel { Id = user.Id });
                }
                else
                {
                    throw new EntityNotFoundDbException();
                }
            }
            catch (EntityNotFoundDbException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.InvalidCredentials.Name,
                                            Message = HandlerFailures.InvalidCredentials,
                                            Target = "credentials"}
                                        }
               );
            }
            catch
            {
                result = Result.Fail(CustomFailures.LoginUserFailure);
            }

            return result;
        }
    }
}
