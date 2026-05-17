namespace LifeQuest.Domain.Tests.Components;

using LifeQuest.Domain.Components;

public class LevelTests
{
    private Level _level;

    [SetUp]
    public void Setup()
    {
        _level = new Level();
    }

    [Test]
    public void UpdateExperience_BelowThreshold_AddsXpWithoutLevelUp()
    {
        _level.UpdateExperience(50);

        Assert.That(_level.CurrentExperience, Is.EqualTo(50));
        Assert.That(_level.LevelValue, Is.EqualTo(1));
    }

    [Test]
    public void UpdateExperience_OverThreshold_LevelsUpAndCarriesRemainder()
    {
        _level.UpdateExperience(200);

        Assert.That(_level.LevelValue, Is.EqualTo(2));
        Assert.That(_level.CurrentExperience, Is.EqualTo(50));
    }

    [Test]
    public void UpdateExperience_NegativeValue_IsIgnored()
    {
        _level.UpdateExperience(-100);

        Assert.That(_level.CurrentExperience, Is.EqualTo(0));
        Assert.That(_level.LevelValue, Is.EqualTo(1));
    }
}