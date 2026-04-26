namespace LifeQuest.Domain.Components;

public class Reward
{
    public int XP { get; } 
    public int Gold { get; }

    public Reward(int xp, int gold)
    {
        XP = xp;
        Gold = gold;
    }
}