using LifeQuest.Application.Interfaces;

namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class PersistenceHandler : BaseQuestHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;

    public PersistenceHandler(IUserRepository userRepository, IQuestRepository questRepository)
    {
        _userRepository = userRepository;
        _questRepository = questRepository;
    }

    public override Task<QuestExecutionResult> Handle(QuestExecutionContext context)
    {
        _userRepository.SaveUser(context.User);
        _questRepository.UpdateQuest(context.Quest);

        return PassToNextAsync(context);
    }
}