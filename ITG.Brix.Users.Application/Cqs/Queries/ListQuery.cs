using ITG.Brix.Users.Application.Bases;
using MediatR;

namespace ITG.Brix.Users.Application.Cqs.Queries
{
    public class ListQuery : IRequest<Result>
    {
        public string Filter { get; private set; }
        public string Top { get; private set; }
        public string Skip { get; private set; }

        public ListQuery(string filter, string top, string skip)
        {
            Filter = filter;
            Top = top;
            Skip = skip;
        }
    }
}
