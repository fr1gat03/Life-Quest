using LifeQuest.Domain.Entities;
using NUnit.Framework;

namespace LifeQuest.Domain.Tests;

public class UserTests
{
    [Test]
    public void UpdateGold_IncreasesUserGold()
    {
        var user = new User(1, "Login", "Pass");
        user.UpdateGold(50);
        Assert.That(user.Gold, Is.EqualTo(50));
    }

    [Test]
    public void UpdateExperience_IncreasesUserXP()
    {
        var user = new User(1, "Login", "Pass");
        user.UpdateExperience(100);
        Assert.That(user.XP, Is.EqualTo(100));
    }
}