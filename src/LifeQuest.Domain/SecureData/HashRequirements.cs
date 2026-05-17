namespace LifeQuest.Domain.SecureData;

public class HashRequirements
{
    private Dictionary<string, PasswordStats> _requirements = new Dictionary<string, PasswordStats>();

    public string GetHash(string login)
    {
        return _requirements[login].Hash;
    }

    public string GetSalt(string login)
    {
        return _requirements[login].Salt;
    }

    public bool AddAccount(string login, string password)
    {
        if (_requirements.ContainsKey(login))
        {
            return false;
        }

        byte[] salt = PasswordHasher.GenerateSalt(password.Length);
        byte[] hash = PasswordHasher.HashPassword(password, salt);

        AddRequirment(login, hash, salt);
        return true;
    }

    private void AddRequirment (string login, byte[] hash, byte[] salt)
    {
        string textHash = Convert.ToBase64String(hash);
        string textSalt = Convert.ToBase64String(salt);

        PasswordStats stats = new PasswordStats(textHash, textSalt);

        _requirements[login] = stats; 
    }
}