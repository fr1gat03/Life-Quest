namespace LifeQuest.Application.Handlers.QuestExecution;

public abstract class BaseQuestHandler
{
    private BaseQuestHandler? _next;

    public BaseQuestHandler SetNext(BaseQuestHandler next)
    {
        _next = next;
        return next;
    }

    public abstract Task<QuestExecutionResult> Handle(QuestExecutionContext context);

    protected Task<QuestExecutionResult> PassToNextAsync(QuestExecutionContext context)
    {
        if (_next is null)
        {
            return Task.FromResult(QuestExecutionResult.Success(context.MotivationMessage ?? string.Empty));
        }

        return _next.Handle(context);
    }
}