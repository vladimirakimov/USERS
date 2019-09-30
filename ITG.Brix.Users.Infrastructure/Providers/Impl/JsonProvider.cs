using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ITG.Brix.Users.Infrastructure.Providers.Impl
{
    public class JsonProvider : IJsonProvider
    {
        public IDictionary<string, string> ToDictionary(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var result = new Dictionary<string, string>(deserialized, StringComparer.InvariantCultureIgnoreCase);
            return result;
        }
    }
}
