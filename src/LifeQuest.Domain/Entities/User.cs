using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class User
{
    public QuestCollection Quests { get; private set; }
    public UserStats UserStats { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public int Streak { get; private set; }

    public User(string login, string passwordHash)
    {
        Quests = new QuestCollection();
        UserStats = new UserStats();

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

public void UpdateExperience(int experience)
    {
        UserStats.Level.UpdateExperience(experience);
    }
}