using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Tests.Fakes;
using LifeQuest.Application.Tests.Helpers;

namespace LifeQuest.Application.Tests.Handlers;

public class PersistenceHandlerTests
{
    private FakeUserRepository _userRepo = null;
    private FakeQuestRepository _questRepo = null;
    private PersistenceHandler _handler = null;

    [SetUp]
    public void Setup()
    {
        _userRepo = new FakeUserRepository();
        _questRepo = new FakeQuestRepository();
        _handler = new PersistenceHandler(_userRepo, _questRepo);
    }

    [Test]
    public async Task Handle_CallsSaveUserWithCorrectUser()
    {
        var user = TestData.NewUser();
        var context = new QuestExecutionContext(TestData.NewQuest(), user);

        await _handler.Handle(context);

        Assert.That(_userRepo.SavedUser, Is.SameAs(user));
    }

    [Test]
    public async Task Handle_CallsUpdateQuestWithCorrectQuest()
    {
        var quest = TestData.NewQuest();
        var context = new QuestExecutionContext(quest, TestData.NewUser());

        await _handler.Handle(context);

        Assert.That(_questRepo.UpdatedQuest, Is.SameAs(quest));
    }
}