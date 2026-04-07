namespace LifeQuest.Domain.Components;

public struct LevelRequirements
{
    private readonly Dictionary<int, int> _requirements;
    public LevelRequirements ()
    {
        _requirements = new Dictionary<int, int>();
        ToInitialize();
    }

    private void ToInitialize ()
    {
        _requirements[1] = 150;
        _requirements[2] = 200;
        _requirements[3] = 250;
        _requirements[4] = 300;
        _requirements[5] = 350;
        _requirements[6] = 450;
        _requirements[7] = 550;
        _requirements[8] = 650;
        _requirements[9] = 750;
        _requirements[10] = 1000;
        _requirements[11] = 1250;
        _requirements[12] = 1500;
        _requirements[13] = 1750;
        _requirements[14] = 2200;
        _requirements[15] = 2700;
        _requirements[16] = 3250;
        _requirements[17] = 3800;
        _requirements[18] = 4250;
        _requirements[19] = 5000;
        _requirements[20] = 6000;
        _requirements[21] = 7000;
        _requirements[22] = 8200;
        _requirements[23] = 9500;
        _requirements[24] = 11000;
        _requirements[25] = 13000;
        _requirements[26] = 15000;
        _requirements[27] = 20000;
    }

    public int GetMaxExpirience (int level)
    {
        bool attempt = _requirements.TryGetValue(level, out int expirience);
        if (!attempt)
        {
            throw new ArgumentNullException();
        }

        return expirience;
    }
}