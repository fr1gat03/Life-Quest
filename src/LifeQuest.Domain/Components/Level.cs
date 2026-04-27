namespace LifeQuest.Domain.Components;

public class Level
{
    public int LevelValue { get; private set; }
    
    public int CurrentExperience { get; private set; }
    public int MaxExperience { get; private set; }

    public Level()
    {
        LevelValue = 1;
        CurrentExperience = 0;
        MaxExperience = GetMaxExperience(LevelValue);
    }

    public void UpdateExperience(int experience)
    {
        if (experience < 0) return;

        CurrentExperience += experience;

        while (CurrentExperience >= MaxExperience)
        {
            CurrentExperience -= MaxExperience;
            LevelValue++;
            MaxExperience = GetMaxExperience(LevelValue);
        }
    }

    private int GetMaxExperience(int level)
    {
        return level * 150;
    }
}