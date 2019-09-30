using AutoMapper;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Application.Internal;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Providers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Cqs.Queries.Handlers
{
    public class ListQueryHandler : IRequestHandler<ListQuery, Result>
    {
        private readonly IMapper _mapper;
        private readonly IUserFinder _userFinder;
        private readonly IOdataProvider _odataProvider;

        public ListQueryHandler(IMapper mapper, IUserFinder userFinder, IOdataProvider odataProvider)
        {
            _mapper = mapper ?? throw Error.ArgumentNull(nameof(mapper));
            _userFinder = userFinder ?? throw Error.ArgumentNull(nameof(userFinder));
            _odataProvider = odataProvider ?? throw Error.ArgumentNull(nameof(odataProvider));
        }

        public async Task<Result> Handle(ListQuery query, CancellationToken cancellationToken)
        {
            Result result;
            try
            {
                Expression<Func<User, bool>> filter = _odataProvider.GetFilterPredicate(query.Filter);
                int? skip = query.Skip.ToNullableInt();
                int? limit = query.Top.ToNullableInt();
                var userDomains = await _userFinder.List(filter, skip, limit);
                var userModels = _mapper.Map<IEnumerable<UserModel>>(userDomains);
                var count = userModels.Count();
                var usersModel = new UsersModel { Value = userModels, Count = count, NextLink = null };

                result = Result.Ok(usersModel);
            }
            catch (FilterODataException)
            {
                result = Result.Fail(new System.Collections.Generic.List<Failure>() {
                                        new HandlerFault(){
                                            Code = HandlerFaultCode.InvalidQueryFilter.Name,
                                            Message = HandlerFailures.InvalidQueryFilter,
                                            Target = "$filter"}
                                        }
                );
            }
            catch
            {
                result = Result.Fail(CustomFailures.ListUserFailure);
            }



            return result;
        }
    }
}
