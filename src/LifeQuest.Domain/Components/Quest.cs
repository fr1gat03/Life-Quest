using LifeQuest.Domain.Enums;

namespace LifeQuest.Domain.Components;

public class Quest
{
    public string Title { get; }
    public bool IsCompleted { get; private set; } = false;
    public Difficulty Difficulty { get; }
    public Reward Reward { get; }

    public Quest(string title, Difficulty difficulty, Reward reward)
    {
        Title = title;
        Difficulty = difficulty;
        Reward = reward;
    }

    public void ToComplete()
    {
        IsCompleted = true;
    }
}