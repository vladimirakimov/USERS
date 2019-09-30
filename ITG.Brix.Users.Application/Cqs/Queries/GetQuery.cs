using ITG.Brix.Users.Application.Bases;
using MediatR;
using System;

namespace ITG.Brix.Users.Application.Cqs.Queries
{
    public class GetQuery : IRequest<Result>
    {
        public Guid Id { get; private set; }

        public GetQuery(Guid id)
        {
            Id = id;
        }
    }
}
