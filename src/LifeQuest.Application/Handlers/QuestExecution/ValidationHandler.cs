namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class ValidationHandler : BaseQuestHandler
{
    public override Task<QuestExecutionResult> Handle(QuestExecutionContext context)
    {
        if (context.Quest is null)
        {
            return Task.FromResult(QuestExecutionResult.Failure("Квест не знайдено"));
        }

        if (context.Quest.IsCompleted)
        {
            return Task.FromResult(QuestExecutionResult.Failure("Квест вже було виконано раніше"));
        }

        if (context.User is null)
        {
            return Task.FromResult(QuestExecutionResult.Failure("Користувача не знайдено"));
        }

        return PassToNextAsync(context);
    }
}