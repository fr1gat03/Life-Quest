using LifeQuest.Domain.Components;
using LifeQuest.Domain.ValueObjects;

namespace LifeQuest.Domain.Entities;

public class User
{
    public UserStats UserStats { get; }
    public string Login { get; }
    public string PasswordHash { get; }
    public int Streak { get; private set; }

    public User (string login, string passwordHash)
    {
        UserStats = new UserStats();

        Login = login;
        PasswordHash = passwordHash;
    }

    public void IncreaseStreak()
    {
        Streak++;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    public bool UpdateHeatPoints (int heatPoints)
    {
        return UserStats.UpdateHeatPoints(heatPoints);
    }

    public bool UpdateGold (int gold)
    {
        return UserStats.UpdateGold(gold);
    }

    public void LevelUp(int expirience)
    {
        UserStats.Level.LevelUp(expirience);
    }

    public void LevelDown(int expirience)
    {
        UserStats.Level.LevelDown(expirience);
    }

    public void UpdateExperience(int experience)
    {
        UserStats.Level.UpdateExperience(experience);
    }
}