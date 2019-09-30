using ITG.Brix.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ITG.Brix.Users.Infrastructure.Providers
{
    public interface IOdataProvider
    {
        string FilterTransform(string filter, IDictionary<string, string> replacements);
        Expression<Func<User, bool>> GetFilterPredicate(string filter);
    }
}
