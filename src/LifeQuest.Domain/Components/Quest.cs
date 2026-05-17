using LifeQuest.Domain.Enums;

namespace LifeQuest.Domain.Entities;

public class Quest
{
    public string Id { get; private set; } 
    public string Title { get; private set; }
    public int RewardXp { get; private set; }
    public int RewardGold { get; private set; }
    public bool IsCompleted { get; private set; }
    public Difficulty Difficulty { get; private set; }

   
    private Quest() { }

    public Quest(string id, string title, int rewardXp, int rewardGold, Difficulty difficulty)
    {
        Id = id;
        Title = title;
        RewardXp = rewardXp;
        RewardGold = rewardGold;
        Difficulty = difficulty;
        IsCompleted = false;
    }

    public void ToComplete()
    {
        IsCompleted = true;
    }
}