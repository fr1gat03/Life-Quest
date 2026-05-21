namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class LevelUpHandler : BaseQuestHandler
{
    public override Task<QuestExecutionResult> Handle(QuestExecutionContext context)
    {
        if (context.LeveledUp)
        {
            var levelUpMessage = $"⬆️ РІВЕНЬ ПІДВИЩЕНО до {context.User.Level}! ";

            context.MotivationMessage = string.IsNullOrEmpty(context.MotivationMessage)
                ? levelUpMessage
                : levelUpMessage + context.MotivationMessage;
        }

        return PassToNextAsync(context);
    }
}