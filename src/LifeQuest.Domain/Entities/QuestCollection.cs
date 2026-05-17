using System;
using System.Collections.Generic;
using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class QuestCollection
{
    public Dictionary<string, Quest> Quests { get; } = new Dictionary<string, Quest>();

    public bool RemoveQuest(string id)
    {
        if (IsIdInKey(id))
        {
            return false;
        }
        Quests.Remove(id);
        return true;
    }

    public bool AddQuest(string id, Quest quest)
    {
        if (!IsValidQuest(quest))
        {
            throw new ArgumentException("Invalid quest");
        }
        if (!IsIdInKey(id))
        {
            return false;
        }
        Quests[id] = quest;
        return true;
    }

    public bool ToComplete(string id)
    {
        if (IsIdInKey(id))
        {
            return false;
        }
        Quest quest = Quests[id];
        if (quest.IsCompleted)
        {
            throw new InvalidOperationException("Quest in completion");
        }
        quest.ToComplete();
        return true;
    }

    private bool IsIdInKey(string id)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }
        return !Quests.ContainsKey(id);
    }

    private bool IsValidId(string id)
    {
        return id != null;
    }

    private bool IsValidQuest(Quest quest)
    {
        bool currentQuest = true;
        if (quest.Title == null || quest.Title == "")
        {
            currentQuest = false;
        }
        return currentQuest;
    }
}