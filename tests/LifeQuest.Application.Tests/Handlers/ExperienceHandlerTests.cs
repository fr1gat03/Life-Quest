using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Tests.Helpers;

namespace LifeQuest.Application.Tests.Handlers;

public class ExperienceHandlerTests
{
    private ExperienceHandler _handler = null;

    [SetUp]
    public void Setup() => _handler = new ExperienceHandler();

    [Test]
    public async Task Handle_AddsXpToUser()
    {
        var user = TestData.NewUser();
        var quest = TestData.NewQuest();
        var context = new QuestExecutionContext(quest, user);
        int xpBefore = user.UserStats.Level.CurrentExperience;

        await _handler.Handle(context);

        Assert.That(user.UserStats.Level.CurrentExperience, Is.EqualTo(xpBefore + quest.RewardXp));
    }

    [Test]
    public async Task Handle_AddsGoldToUser()
    {
        var user = TestData.NewUser();
        var quest = TestData.NewQuest();
        var context = new QuestExecutionContext(quest, user);
        int goldBefore = user.UserStats.Gold;

        await _handler.Handle(context);

        Assert.That(user.UserStats.Gold, Is.EqualTo(goldBefore + quest.RewardGold));
    }

    [Test]
    public async Task Handle_MarksQuestAsCompleted()
    {
        var quest = TestData.NewQuest();
        var context = new QuestExecutionContext(quest, TestData.NewUser());

        await _handler.Handle(context);

        Assert.That(quest.IsCompleted, Is.True);
    }
}