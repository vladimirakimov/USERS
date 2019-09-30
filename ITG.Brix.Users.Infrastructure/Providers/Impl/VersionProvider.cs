using System;
using System.Text.RegularExpressions;

namespace ITG.Brix.Users.Infrastructure.Providers.Impl
{
    public class VersionProvider : IVersionProvider
    {
        public int Generate()
        {
            int min = 1000000000;
            int max = int.MaxValue;
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            var result = new Random(seed).Next(min, max);
            return result;
        }
    }
}
