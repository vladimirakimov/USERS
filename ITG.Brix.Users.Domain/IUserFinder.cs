using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Domain
{
    public interface IUserFinder
    {
        Task<IEnumerable<User>> List(Expression<Func<User, bool>> filter, int? skip, int? limit);
        Task<User> Get(Guid id);
        Task<User> Get(string login);
        Task<bool> Exists(string login);
    }
}
