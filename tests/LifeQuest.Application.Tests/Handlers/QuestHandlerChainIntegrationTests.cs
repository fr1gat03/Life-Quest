using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Tests.Fakes;
using LifeQuest.Application.Tests.Helpers;

namespace LifeQuest.Application.Tests.Handlers;

public class QuestHandlerChainIntegrationTests
{
    private FakeAiService _aiService = null;
    private FakeUserRepository _userRepo = null;
    private FakeQuestRepository _questRepo = null;

    [SetUp]
    public void Setup()
    {
        _aiService = new FakeAiService();
        _userRepo = new FakeUserRepository();
        _questRepo = new FakeQuestRepository();
    }

    [Test]
    public async Task FullChain_ValidQuest_ReturnsSuccessWithMotivation()
    {
        var user = TestData.NewUser();
        var quest = TestData.NewQuest();
        var chain = QuestHandlerChainBuilder.Build(_aiService, _userRepo, _questRepo);
        var context = new QuestExecutionContext(quest, user);

        QuestExecutionResult result = await chain.Handle(context);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.MotivationMessage, Is.EqualTo(FakeAiService.FakeMessage));
        Assert.That(quest.IsCompleted, Is.True);
        Assert.That(user.UserStats.Level.CurrentExperience, Is.EqualTo(50));
        Assert.That(user.UserStats.Gold, Is.EqualTo(10));
    }

    [Test]
    public async Task FullChain_AlreadyCompletedQuest_StopsAtValidation()
    {
        var chain = QuestHandlerChainBuilder.Build(_aiService, _userRepo, _questRepo);
        var context = new QuestExecutionContext(
            TestData.NewQuest(isCompleted: true),
            TestData.NewUser());

        QuestExecutionResult result = await chain.Handle(context);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(_userRepo.SavedUser, Is.Null);
    }
}