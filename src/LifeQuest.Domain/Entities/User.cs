using System;
using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class LevelInfo
{
    public int LevelValue { get; set; } = 1;
    public int CurrentExperience { get; set; } = 0;
    public int MaxExperience { get; set; } = 100;
}

public class UserStats
{
    public int HealthPoints { get; set; } = 100;
    public int Gold { get; set; } = 0;
    public LevelInfo Level { get; set; } = new LevelInfo();
}

public class User
{
    public UserStats UserStats { get; private set; } = new UserStats();

    // ЯРЛИКИ (Shorthands) - щоб працювали і тести, і UI
    public int Gold => UserStats.Gold;
    public int XP => UserStats.Level.CurrentExperience;
    public int HealthPoints => UserStats.HealthPoints;
    public int Level => UserStats.Level.LevelValue;

    public int Id { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public QuestCollection Quests { get; private set; }
    public int Streak { get; private set; }

    public User(int id, string login, string passwordHash)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Quests = new QuestCollection();
    }

    private User() { Quests = new QuestCollection(); }

    public void UpdateHealth(int amount)
    {
        UserStats.HealthPoints = Math.Clamp(UserStats.HealthPoints + amount, 0, 100);
    }

    public void UpdateExperience(int amount)
    {
        var lvl = UserStats.Level;
        lvl.CurrentExperience += amount;
        while (lvl.CurrentExperience >= lvl.MaxExperience)
        {
            lvl.CurrentExperience -= lvl.MaxExperience;
            lvl.LevelValue++;
            lvl.MaxExperience = (int)(lvl.MaxExperience * 1.2);
        }
    }

    public void UpdateGold(int amount) => UserStats.Gold += amount;
    public void IncreaseStreak() => Streak++;
    public void ResetStreak() => Streak = 0;
}