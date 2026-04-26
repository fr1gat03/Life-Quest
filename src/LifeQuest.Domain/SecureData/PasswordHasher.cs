using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace LifeQuest.Domain.SecureData;

public class PasswordHasher
{
    public static byte[] HashPassword(string password, byte[] salt)
    {
        using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            hasher.Salt = salt;
            hasher.DegreeOfParallelism = 8; 
            hasher.MemorySize = 65536; 
            hasher.Iterations = 4; 
            return hasher.GetBytes(32); 
        }
    }

    public static byte[] GenerateSalt(int length)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(length);
        return salt;
    }
}