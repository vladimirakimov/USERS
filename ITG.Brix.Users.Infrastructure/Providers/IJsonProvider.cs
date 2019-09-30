using System.Collections.Generic;

namespace ITG.Brix.Users.Infrastructure.Providers
{
    public interface IJsonProvider
    {
        IDictionary<string, string> ToDictionary(string json);
    }
}
