using LifeQuest.Domain.Components;

namespace LifeQuest.Application.Interfaces;

public interface IQuestRepository
{
    IEnumerable<Quest> GetActiveQuests(int userId);
    void UpdateQuest(Quest quest);
}