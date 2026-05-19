using System.Threading.Tasks;
using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using Moq;
using NUnit.Framework;

namespace LifeQuest.Application.Tests;

public class QuestHandlerChainIntegrationTests
{
    [Test]
    public async Task FullChain_UpdatesUserAndSavesToDb()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var questRepoMock = new Mock<IQuestRepository>();

        var user = new User(1, "Login", "Pass");
        var quest = new Quest("1", "Title", 100, 50, Difficulty.Medium);
        var context = new QuestExecutionContext(quest, user);

        var validation = new ValidationHandler();
        var experience = new ExperienceHandler();
        var persistence = new PersistenceHandler(userRepoMock.Object, questRepoMock.Object);

        validation.SetNext(experience);
        experience.SetNext(persistence);

        await validation.Handle(context);

        Assert.That(user.XP, Is.EqualTo(100));

        userRepoMock.Verify(r => r.SaveUser(It.IsAny<User>()), Times.Once);
        questRepoMock.Verify(r => r.UpdateQuest(It.IsAny<Quest>()), Times.Once);
    }
}