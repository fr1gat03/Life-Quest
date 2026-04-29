namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class ExperienceHandler : BaseQuestHandler
{
    public override Task<QuestExecutionResult> Handle(QuestExecutionContext context)
    {
        context.User.UpdateExperience(context.Quest.RewardXp);
        context.User.UpdateGold(context.Quest.RewardGold);
        context.Quest.ToComplete();

        return PassToNextAsync(context);
    }
}