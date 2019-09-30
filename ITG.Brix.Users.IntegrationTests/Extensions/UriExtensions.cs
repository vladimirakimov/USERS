using System;

namespace ITG.Brix.Users.IntegrationTests.Extensions
{
    public static class UriExtensions
    {

        public static Guid GetId(this Uri uri)
        {
            var idAsString = uri.OriginalString.Substring(uri.OriginalString.LastIndexOf("/") + 1);
            return new Guid(idAsString);
        }
    }
}
