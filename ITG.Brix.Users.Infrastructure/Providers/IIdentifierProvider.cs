using System;

namespace ITG.Brix.Users.Infrastructure.Providers
{
    public interface IIdentifierProvider
    {
        Guid Generate();
    }
}
