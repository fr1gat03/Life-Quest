namespace LifeQuest.Domain.Tests.Entities;

using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

public class UserTests
{
    private User _user;
    private Quest _quest;

    [SetUp]
    public void Setup()
    {
        _user = new User(1, "hero", "hash");
        _quest = new Quest("Квест", 100, 50, Difficulty.Hard);
    }

    [Test]
    public void IncreaseStreak_IncreasesStreakByOne()
    {
        _user.IncreaseStreak();

        Assert.That(_user.Streak, Is.EqualTo(1));
    }

    [Test]
    public void ResetStreak_SetsStreakToZero()
    {
        _user.IncreaseStreak();
        _user.ResetStreak();

        Assert.That(_user.Streak, Is.EqualTo(0));
    }

    [Test]
    public void UpdateGold_AddsGoldToUserStats()
    {
        _user.UpdateGold(100);

        Assert.That(_user.UserStats.Gold, Is.EqualTo(100));
    }

    [Test]
    public void UpdateExperience_AddsExperienceToLevel()
    {
        _user.UpdateExperience(50);

        Assert.That(_user.UserStats.Level.CurrentExperience, Is.EqualTo(50));
    }
}