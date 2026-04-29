namespace LifeQuest.Domain.Tests.Entities;

using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

public class QuestCollectionTests
{
    private QuestCollection _collection;
    private Quest _quest;

    [SetUp]
    public void Setup()
    {
        _collection = new QuestCollection();
        _quest = new Quest("Тест", 50, 10, Difficulty.Easy);
    }

    [Test]
    public void AddQuest_DuplicateId_ReturnsFalse()
    {
        _collection.AddQuest("q1", _quest);

        bool result = _collection.AddQuest("q1", _quest);

        Assert.That(result, Is.False);
    }

    [Test]
    public void AddQuest_NullId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _collection.AddQuest(null, _quest));
    }

    [Test]
    public void ToComplete_MarksQuestAsCompleted()
    {
        _collection.AddQuest("q1", _quest);

        _collection.ToComplete("q1");

        Assert.That(_quest.IsCompleted, Is.True);
    }

    [Test]
    public void ToComplete_AlreadyCompletedQuest_ThrowsInvalidOperationException()
    {
        _collection.AddQuest("q1", _quest);
        _collection.ToComplete("q1");

        Assert.Throws<InvalidOperationException>(() => _collection.ToComplete("q1"));
    }

    [Test]
    public void RemoveQuest_NonExistingId_ReturnsFalse()
    {
        bool result = _collection.RemoveQuest("q999");

        Assert.That(result, Is.False);
    }
}