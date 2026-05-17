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
        return _context.Quests.Where(q => !q.IsCompleted).ToList();
    }

    public void UpdateQuest(Quest quest)
    {
        _context.Quests.Update(quest);
        _context.SaveChanges();
    }
}