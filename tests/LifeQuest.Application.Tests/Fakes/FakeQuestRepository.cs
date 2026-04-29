using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Components;

namespace LifeQuest.Application.Tests.Fakes;

public sealed class FakeQuestRepository : IQuestRepository
{
    public Quest UpdatedQuest { get; private set; }

    public void UpdateQuest(Quest quest) => UpdatedQuest = quest;
    public IEnumerable<Quest> GetActiveQuests(int userId) => [];
}