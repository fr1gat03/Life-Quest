namespace LifeQuest.Domain.Tests.Components;

using LifeQuest.Domain.Components;
using LifeQuest.Domain.Enums;

public class QuestTests
{
    private Quest _quest;

    [SetUp]
    public void Setup()
    {
        _quest = new Quest("Знайти артефакт", 100, 25, Difficulty.Medium);
    }

    [Test]
    public void ToComplete_SetsIsCompletedToTrue()
    {
        _quest.ToComplete();

        Assert.That(_quest.IsCompleted, Is.True);
    }
}