using ITG.Brix.Users.Application.DataTypes;
using System.Collections.Generic;

namespace ITG.Brix.Users.Application.Extensions
{
    public static class IDictionaryExtensions
    {
        public static Optional<string> GetOptional(this IDictionary<string, string> dictionary, string key)
        {
            Optional<string> result = dictionary.ContainsKey(key) ? new Optional<string>(dictionary[key]) : new Optional<string>();

            return result;
        }
    }
}
