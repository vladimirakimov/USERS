namespace ITG.Brix.Users.Infrastructure.Providers
{
    public interface IPasswordProvider
    {
        /// <summary>
        /// Creates hashed password.
        /// </summary>
        string Hash(string password);

        /// <summary>
        /// Verifies password against hash.
        /// </summary>
        bool Verify(string password, string hashedPassword);
    }
}
