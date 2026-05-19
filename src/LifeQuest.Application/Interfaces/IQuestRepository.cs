using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Interfaces;

public interface IQuestRepository
{
    IEnumerable<Quest> GetActiveQuests(int userId);
    Quest? GetQuestById(string id);
    void UpdateQuest(Quest quest);
}