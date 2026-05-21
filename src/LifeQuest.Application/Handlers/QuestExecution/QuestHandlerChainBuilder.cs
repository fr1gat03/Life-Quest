using LifeQuest.Application.Interfaces;

namespace LifeQuest.Application.Handlers.QuestExecution;

public static class QuestHandlerChainBuilder
{
    public static BaseQuestHandler Build(
        IAiService aiService,
        IUserRepository userRepository,
        IQuestRepository questRepository)
    {
        var validation  = new ValidationHandler();
        var experience  = new ExperienceHandler();
        var levelUp     = new LevelUpHandler();
        var motivation  = new AiMotivationHandler(aiService);
        var persistence = new PersistenceHandler(userRepository, questRepository);

        validation
            .SetNext(experience)
            .SetNext(levelUp)
            .SetNext(motivation)
            .SetNext(persistence);

        return validation;
    }
}