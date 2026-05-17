using LifeQuest.Application.Interfaces;

namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class AiMotivationHandler : BaseQuestHandler
{
    private readonly IAiService _aiService;

    public AiMotivationHandler(IAiService aiService)
    {
        _aiService = aiService;
    }

    public override async Task<QuestExecutionResult> Handle(QuestExecutionContext context)
    {
        context.MotivationMessage = await _aiService.GenerateMotivationMessage(context.Quest.Title);
        return await PassToNextAsync(context);
    }
}