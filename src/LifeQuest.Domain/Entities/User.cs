using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class User
{
    private QuestCollection _quests;
    private UserStats _userStats;
    public string Login { get; }
    public string PasswordHash { get; }
    public int Streak { get; private set; }

    public User(string login, string passwordHash)
    {
        _quests = new QuestCollection();
        _userStats = new UserStats();

        Login = login;
        PasswordHash = passwordHash;
    }

    public bool RemoveQuest(string id)
    {
        return _quests.RemoveQuest(id);
    }

    public bool AddQuest(string id, Quest quest)
    {
        return _quests.AddQuest(id, quest);
    }

    public bool ToComplete(string id)
    {
        return _quests.ToComplete(id);
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
        _userStats.UpdateHeatPoints(heatPoints);
    }

    public void UpdateGold(int gold)
    {
        _userStats.UpdateGold(gold);
    }

    public void UpdateExperience(int experience)
    {
        _userStats.Level.UpdateExperience(experience);
    }
}