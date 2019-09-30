using System;
using System.Security.Cryptography;

namespace ITG.Brix.Users.Infrastructure.Providers.Impl
{
    /// <remarks>
    /// Based on https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    /// </remarks>
    public class PasswordProvider : IPasswordProvider
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int NumberOfIterations = 10000;

        public string Hash(string password)
        {
            var result = Hash(password, NumberOfIterations);

            return result;
        }

        public bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static string Hash(string password, int iterations)
        {
            //create salt
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        private static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }
    }
}
