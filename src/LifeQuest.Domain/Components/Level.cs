namespace LifeQuest.Domain.Components;

public class Level
{
    private readonly LevelRequirements _requirements = new LevelRequirements();
    public int LevelValue { get; private set; }
    public int CurrentExpirience { get; private set; }
    public int MaxExpirience { get; private set; }

    public Level()
    {
        LevelValue = 1;
        MaxExpirience = _requirements.GetMaxExpirience(LevelValue);
    }

    public void LevelUp(int expirience)
    {
        int deltaExpirience = expirience - (MaxExpirience - CurrentExpirience);

        LevelValue++;

        CurrentExpirience = deltaExpirience;
        MaxExpirience = _requirements.GetMaxExpirience(LevelValue);
    }

    public void LevelDown(int expirience)
    {
        int previousMaxExperience = _requirements.GetMaxExpirience(LevelValue - 1);
        int deltaExpirience = previousMaxExperience - (expirience - CurrentExpirience);

        LevelValue--;

        CurrentExpirience = deltaExpirience;
        MaxExpirience = previousMaxExperience;
    }

    public void UpdateExperience(int experience)
    {
        bool shouldLevelUp = CurrentExpirience + experience >= MaxExpirience;
        bool shouldLevelDown = CurrentExpirience + experience <= 0;

        if (shouldLevelUp)
        {
            LevelUp(experience);
        }
        else if (shouldLevelDown)
        {
            LevelDown(experience);

        }
    }
}