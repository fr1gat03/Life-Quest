using LifeQuest.Domain.Enums;

namespace LifeQuest.Domain.Components;

public class Quest
{
    public string Title { get; private set; }
    public int RewardXp { get; private set; }
    public int RewardGold { get; private set; }
    public bool IsCompleted { get; private set; }
    public Difficulty Difficulty { get; private set; }

    public Quest(string title, int rewardXp, int rewardGold, Difficulty difficulty)
    {
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