namespace LifeQuest.Domain.Components;

public class UserStats
{   
    public Level Level { get; }
    public int HeatPoints { get; private set; }
    public int Gold { get; private set; }

    public UserStats ()
    {
        Level = new Level();
        HeatPoints = 100;
    }

    public bool UpdateHeatPoints (int heatPoints)
    {
        if (HeatPoints + heatPoints > 100)
        {
            HeatPoints = 100;
        }
        else if (HeatPoints + heatPoints < 0)
        {
            HeatPoints = 0;
        }
        else
        {
            HeatPoints += heatPoints;
        }
        
        return true;
    }

    public bool UpdateGold (int gold)
    {
        Gold += gold;
        return true;
    }
}