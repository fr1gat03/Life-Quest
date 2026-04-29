using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Tests.Fakes;
using LifeQuest.Application.Tests.Helpers;

namespace LifeQuest.Application.Tests.Handlers;

public class AiMotivationHandlerTests
{
    private AiMotivationHandler _handler = null;

    [SetUp]
    public void Setup() => _handler = new AiMotivationHandler(new FakeAiService());

    [Test]
    public async Task Handle_WritesMotivationMessageToContext()
    {
        var context = new QuestExecutionContext(TestData.NewQuest(), TestData.NewUser());

        await _handler.Handle(context);

        Assert.That(context.MotivationMessage, Is.EqualTo(FakeAiService.FakeMessage));
    }

    [Test]
    public async Task Handle_ReturnsSuccessWithMotivationMessage()
    {
        var context = new QuestExecutionContext(TestData.NewQuest(), TestData.NewUser());

        QuestExecutionResult result = await _handler.Handle(context);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.MotivationMessage, Is.EqualTo(FakeAiService.FakeMessage));
    }
}