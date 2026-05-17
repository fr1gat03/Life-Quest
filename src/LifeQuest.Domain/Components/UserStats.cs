namespace LifeQuest.Domain.Components;

public class UserStats
{   
    public Level Level { get; }
    public int HealthPoints { get; private set; }
    public int Gold { get; private set; }

    public UserStats ()
    {
        Level = new Level();
        HealthPoints = 100;
    }

    public void UpdateHeatPoints (int hp)
    {
        if (HealthPoints + hp > 100)
        {
            HealthPoints = 100;
        }
        else if (HealthPoints + hp < 0)
        {
            HealthPoints = 0;
        }
        else
        {
            HealthPoints += hp;
        }
    }

    public void UpdateGold (int gold)
    {
        Gold += gold;
    }
}