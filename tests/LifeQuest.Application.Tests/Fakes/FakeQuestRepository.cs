using System.Collections.Generic;
using System.Linq;
using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Tests;

public class FakeQuestRepository : IQuestRepository
{
    private readonly List<Quest> _quests = new();

    public IEnumerable<Quest> GetActiveQuests(int userId)
    {
        return _quests.Where(q => !q.IsCompleted);
    }

    public Quest? GetQuestById(string id)
    {
        return _quests.FirstOrDefault(q => q.Id == id);
    }

    public void UpdateQuest(Quest quest)
    {
        var existing = _quests.FirstOrDefault(q => q.Id == quest.Id);
        if (existing != null) _quests.Remove(existing);
        _quests.Add(quest);
    }
}