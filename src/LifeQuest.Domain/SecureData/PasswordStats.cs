using System;
using System.Collections.Generic;
using System.Text;

namespace LifeQuest.Domain.SecureData;

public class PasswordStats
{
    public string Hash { get; }
    public string Salt { get; }

    public PasswordStats(string hash, string salt)
    {
        Hash = hash;
        Salt = salt;
    }
}