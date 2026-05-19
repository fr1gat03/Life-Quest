using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using NUnit.Framework;

namespace LifeQuest.Application.Tests;

public class ValidationHandlerTests
{
    [Test]
    public async Task Handle_ValidContext_PassesToNext()
    {
        var user = new User(1, "Login", "Pass");
        var quest = new Quest("1", "Title", 100, 50, Difficulty.Easy);
        var context = new QuestExecutionContext(quest, user);
        var handler = new ValidationHandler();

        
        var result = await handler.Handle(context);

        Assert.That(result, Is.Not.Null);
    }
}