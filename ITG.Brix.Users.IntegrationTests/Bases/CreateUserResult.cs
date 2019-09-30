using System;

namespace ITG.Brix.Users.IntegrationTests.Bases
{
    public class CreateUserResult
    {
        public CreateUserResult(Guid id, string eTag)
        {
            Id = id;
            ETag = eTag;
        }
        public Guid Id { get; private set; }
        public string ETag { get; private set; }
    }
}
