using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Tests.Helpers;

namespace LifeQuest.Application.Tests.Handlers;

public class ValidationHandlerTests
{
    private ValidationHandler _handler = null;

    [SetUp]
    public void Setup() => _handler = new ValidationHandler();

    [Test]
    public async Task Handle_ValidQuest_PassesToNext()
    {
        var context = new QuestExecutionContext(TestData.NewQuest(), TestData.NewUser());

        QuestExecutionResult result = await _handler.Handle(context);

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task Handle_AlreadyCompletedQuest_ReturnsFailure()
    {
        var context = new QuestExecutionContext(TestData.NewQuest(true), TestData.NewUser());

        QuestExecutionResult result = await _handler.Handle(context);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task Handle_NullQuest_ReturnsFailure()
    {
        var context = new QuestExecutionContext(null, TestData.NewUser());

        QuestExecutionResult result = await _handler.Handle(context);

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task Handle_NullUser_ReturnsFailure()
    {
        var context = new QuestExecutionContext(TestData.NewQuest(), null);

        QuestExecutionResult result = await _handler.Handle(context);

        Assert.That(result.IsSuccess, Is.False);
    }
}