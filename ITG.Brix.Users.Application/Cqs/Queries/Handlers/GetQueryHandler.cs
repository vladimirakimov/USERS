using AutoMapper;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Internal;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Cqs.Queries.Handlers
{
    public class GetQueryHandler : IRequestHandler<GetQuery, Result>
    {
        private readonly IMapper _mapper;
        private readonly IUserFinder _userFinder;

        public GetQueryHandler(IMapper mapper, IUserFinder userFinder)
        {
            _mapper = mapper ?? throw Error.ArgumentNull(nameof(mapper));
            _userFinder = userFinder ?? throw Error.ArgumentNull(nameof(userFinder));
        }

        public async Task<Result> Handle(GetQuery query, CancellationToken cancellationToken)
        {
            Result result;

            try
            {
                var user = await _userFinder.Get(query.Id);
                var userModel = _mapper.Map<UserModel>(user);

                result = Result.Ok(userModel, user.Version);
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
            catch
            {
                result = Result.Fail(CustomFailures.GetUserFailure);
            }

            return result;
        }
    }
}
