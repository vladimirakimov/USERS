using System;

namespace ITG.Brix.Users.Infrastructure.Providers.Impl
{
    public class IdentifierProvider : IIdentifierProvider
    {
        public Guid Generate()
        {
            var result = Guid.NewGuid();

            return result;
        }
    }
}
