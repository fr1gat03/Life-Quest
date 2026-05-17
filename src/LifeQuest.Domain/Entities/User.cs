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
﻿using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class User
{
    public QuestCollection Quests { get; private set; }
    public UserStats UserStats { get; private set; }
    public int Id { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public int Streak { get; private set; }

    public User(int id, string login, string passwordHash)
    {
        Quests = new QuestCollection();
        UserStats = new UserStats();

        Id = id;
        Login = login;
        PasswordHash = passwordHash;      
    }

    public bool RemoveQuest(string id)
    {
        return Quests.RemoveQuest(id);
    }

    public bool AddQuest(string id, Quest quest)
    {
        return Quests.AddQuest(id, quest);
    }

    public bool ToComplete(string id)
    {
        return Quests.ToComplete(id);
    }

    public void IncreaseStreak()
    {
        Streak++;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    public void UpdateHeatPoints(int heatPoints)
    {
        UserStats.UpdateHeatPoints(heatPoints);
    }

    public void UpdateGold(int gold)
    {
        UserStats.UpdateGold(gold);
    }

    public void UpdateExperience(int experience)
    {
        UserStats.Level.UpdateExperience(experience);
    }
}