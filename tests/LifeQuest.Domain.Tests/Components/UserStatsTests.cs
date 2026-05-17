namespace LifeQuest.Domain.Tests.Components;

using LifeQuest.Domain.Components;

public class UserStatsTests
{
    private UserStats _stats;

    [SetUp]
    public void Setup()
    {
        _stats = new UserStats();
    }

    [Test]
    public void UpdateGold_AddsCorrectAmount()
    {
        _stats.UpdateGold(50);

        Assert.That(_stats.Gold, Is.EqualTo(50));
    }

    [Test]
    public void UpdateHeatPoints_DecreasesHealthPoints()
    {
        _stats.UpdateHeatPoints(-30);

        Assert.That(_stats.HealthPoints, Is.EqualTo(70));
    }

    [Test]
    public void UpdateHeatPoints_CannotGoBelowZero()
    {
        _stats.UpdateHeatPoints(-200);

        Assert.That(_stats.HealthPoints, Is.EqualTo(0));
    }
}