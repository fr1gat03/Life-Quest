using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;
using LifeQuest.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace LifeQuest.Infrastructure.Repositories;

public class QuestRepository : IQuestRepository
{
    private readonly LifeQuestDbContext _context;

    public QuestRepository(LifeQuestDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Quest> GetActiveQuests(int userId)
    {
        return _context.Quests
            .Where(q => !q.IsCompleted && q.UserId == userId)
            .ToList();
    }
    
    public int GetCompletedQuestsCount(int userId)
    {
        return _context.Quests.Count(q => q.UserId == userId && q.IsCompleted);
    }

    public Quest? GetQuestById(string id)
    {
        return _context.Quests.Find(id);
    }
    
    public void DeleteAllUserQuests(int userId)
    {
        var quests = _context.Quests.Where(q => q.UserId == userId).ToList();
        _context.Quests.RemoveRange(quests);
        _context.SaveChanges();
    }

    public void UpdateQuest(Quest quest)
    {
        var existing = _context.Quests.Find(quest.Id);

        if (existing == null)
        {
            _context.Quests.Add(quest);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(quest);
        }
        _context.SaveChanges();
    }
    
    public bool HasAnyQuests(int userId)
    {
        return _context.Quests.Any(q => q.UserId == userId);
    }
}