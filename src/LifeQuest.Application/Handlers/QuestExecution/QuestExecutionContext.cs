using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Handlers.QuestExecution;

public sealed class QuestExecutionContext
{
    public Quest Quest { get; }
    public User User { get; }
    public string? MotivationMessage { get; set; }

    public QuestExecutionContext(Quest quest, User user)
    {
        Quest = quest;
        User = user;
    }
}