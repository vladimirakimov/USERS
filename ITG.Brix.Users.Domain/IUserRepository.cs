using System;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Domain
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task Update(User user);
        Task Delete(Guid id, int version);
    }
}
