using ITG.Brix.Users.Application.Bases;
using MediatR;
using System;

namespace ITG.Brix.Users.Application.Cqs.Commands
{
    public class DeleteCommand : IRequest<Result>
    {
        public Guid Id { get; private set; }

        public int Version { get; private set; }

        public DeleteCommand(Guid id, int version)
        {
            Id = id;
            Version = version;
        }
    }

}
