using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public UserStats UserStats { get; }
    public string Login { get; }
    public string PasswordHash { get; }
    public int Streak { get; private set; }

   
    private User()
    {
        UserStats = new UserStats();
    }

    public User(string login, string passwordHash)
    {
        UserStats = new UserStats();
        Login = login;
        PasswordHash = passwordHash;
        Id = 0;
    }

    public void IncreaseStreak() => Streak++;

    public void ResetStreak() => Streak = 0;

    public bool UpdateHealthPoints(int heatPoints)
    {
        return UserStats.UpdateHeatPoints(heatPoints);
    }

    public bool UpdateGold(int gold)
    {
        return UserStats.UpdateGold(gold);
    }

    public void LevelUp(int experience)
    {
        UserStats.Level.LevelUp(experience);
    }

    public void LevelDown(int experience)
    {
        UserStats.Level.LevelDown(experience);
    }

    public void UpdateExperience(int experience)
    {
        UserStats.Level.UpdateExperience(experience);
    }
}