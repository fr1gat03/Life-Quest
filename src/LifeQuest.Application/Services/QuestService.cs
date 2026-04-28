using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Services;

public class QuestService
{
    private readonly IAiService _aiService;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;

    public QuestService(
        IAiService aiService,
        IUserRepository userRepository,
        IQuestRepository questRepository)
    {
        _aiService = aiService;
        _userRepository = userRepository;
        _questRepository = questRepository;
    }

    public async Task<QuestExecutionResult> CompleteQuestAsync(Quest quest, User user)
    {
        var context = new QuestExecutionContext(quest, user);
        var chain = QuestHandlerChainBuilder.Build(_aiService, _userRepository, _questRepository);
        return await chain.Handle(context);
    }
}