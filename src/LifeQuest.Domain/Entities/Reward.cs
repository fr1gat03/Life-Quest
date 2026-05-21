using LifeQuest.Domain.Entities;

namespace LifeQuest.Domain.Entities;

public class Reward
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Cost { get; private set; }

    private Reward() { Id = ""; Name = ""; Description = ""; }

    public Reward(string id, string name, int cost, string description = "")
    {
        Id = id;
        Name = name;
        Cost = cost;
        Description = description;
    }

    public bool Purchase(User user)
    {
        if (user.Gold < Cost)
            return false;

        user.UpdateGold(-Cost);
        return true;
    }
}