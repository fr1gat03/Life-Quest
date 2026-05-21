using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Interfaces;

public interface IQuestRepository
{
    IEnumerable<Quest> GetActiveQuests(int userId);
    Quest? GetQuestById(string id);
    bool HasAnyQuests(int userId);
    void UpdateQuest(Quest quest);
    void DeleteAllUserQuests(int userId); 
}