using System.Threading.Tasks;
using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using NUnit.Framework;

namespace LifeQuest.Application.Tests;

public class ExperienceHandlerTests
{
    [Test]
    public async Task Handle_AddsExperienceToUser()
    {
        var user = new User(1, "Login", "Pass");
        var quest = new Quest("1", "Title", 100, 50, Difficulty.Easy);
        var context = new QuestExecutionContext(quest, user);
        var handler = new ExperienceHandler();

        await handler.Handle(context);

        Assert.That(user.XP, Is.EqualTo(100));
    }
}