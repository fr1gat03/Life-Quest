using NUnit.Framework;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.Domain.Tests.Components;

public class QuestTests
{
    private Quest _quest;

    [SetUp]
    public void Setup()
    {
        _quest = new Quest("1", "Знайти артефакт", 100, 25, Difficulty.Medium);
    }

    [Test]
    public void ToComplete_SetsIsCompletedToTrue()
    {
        _quest.ToComplete();

        Assert.That(_quest.IsCompleted, Is.True);
    }
}