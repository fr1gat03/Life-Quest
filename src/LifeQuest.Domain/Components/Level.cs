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

    private void LevelUp(int experience)
    {
        int deltaExperience = experience - (MaxExperience - CurrentExperience);

        LevelValue++;

        CurrentExperience = deltaExperience;                              
        MaxExperience = GetMaxExperience(LevelValue);
    }

    private void LevelDown(int experience)
    {
        int previousMaxExperience = GetMaxExperience(LevelValue - 1);
        int deltaExperience = previousMaxExperience + experience + CurrentExperience;

        LevelValue--;

        CurrentExperience = deltaExperience;
        MaxExperience = previousMaxExperience;
    }

    public void UpdateExperience(int experience)
    {
        bool shouldLevelUp = CurrentExperience + experience >= MaxExperience;
        bool shouldLevelDown = CurrentExperience + experience < 0;

        CurrentExperience += experience;

        while (CurrentExperience >= MaxExperience)
        {
            CurrentExperience -= MaxExperience;
            LevelValue++;
            MaxExperience = GetMaxExperience(LevelValue);
        }
        else if (shouldLevelDown)
        {
            if (LevelValue == 1)
            {
                CurrentExperience = 0;
            }
            else
            {
                LevelDown(experience);
            }
        }
        else
        {
            CurrentExperience += experience;
        }
    }

    private int GetMaxExperience(int level)
    {
        return level * 150;
    }
}