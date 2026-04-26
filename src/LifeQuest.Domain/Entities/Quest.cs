namespace LifeQuest.Domain.Entities;

// Тимчасовий клас
public class Quest
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public int RewardXp { get; private set; }
    public int RewardGold { get; private set; }
    public bool IsCompleted { get; private set; }

    public Quest(string title, int rewardXp, int rewardGold)
    {
        Title = title;
        RewardXp = rewardXp;
        RewardGold = rewardGold;
        IsCompleted = false;
    }
}