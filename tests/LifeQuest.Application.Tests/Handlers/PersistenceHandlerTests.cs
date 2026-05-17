using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.Application.Tests;

public class PersistenceHandlerTests
{
    [Test]
    public async Task Handle_CallsRepositorySaveMethods()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var questRepoMock = new Mock<IQuestRepository>();
        var user = new User(1, "Login", "Pass");
        var quest = new Quest("1", "Title", 100, 50, Difficulty.Easy);
        var context = new QuestExecutionContext(quest, user);
        var handler = new PersistenceHandler(userRepoMock.Object, questRepoMock.Object);

        await handler.Handle(context);

        userRepoMock.Verify(r => r.SaveUser(It.IsAny<User>()), Times.Once);
        questRepoMock.Verify(r => r.UpdateQuest(It.IsAny<Quest>()), Times.Once);
    }
}