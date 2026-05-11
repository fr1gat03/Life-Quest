using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using NUnit.Framework;

namespace LifeQuest.Domain.Tests;

public class QuestCollectionTests
{
    [Test]
    public void AddQuest_IncreasesCount()
    {
        var collection = new QuestCollection();
        var quest = new Quest("1", "Title", 100, 50, Difficulty.Easy);

        collection.AddQuest("1", quest);

        Assert.That(collection.Quests.Count, Is.EqualTo(1));
    }
}