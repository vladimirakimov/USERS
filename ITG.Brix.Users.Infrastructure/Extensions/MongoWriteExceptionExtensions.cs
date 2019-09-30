using MongoDB.Driver;

namespace ITG.Brix.Users.Infrastructure.Extensions
{
    public static class MongoWriteExceptionExtensions
    {
        private static readonly int MongoUniqueViolationCode = 11000;

        public static bool IsUniqueViolation(this MongoWriteException exception)
        {
            var result = false;
            if (exception != null && exception.WriteError != null)
            {
                result = exception.WriteError.Code.Equals(MongoUniqueViolationCode);
            }
            return result;
        }
    }
}
