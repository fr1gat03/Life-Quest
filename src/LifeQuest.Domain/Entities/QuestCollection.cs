using System;
using System.Collections.Generic;
using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class QuestCollection
{
    // Змінено на Quests (з великої літери) та зроблено властивістю
    public Dictionary<string, Quest> Quests { get; } = new Dictionary<string, Quest>();

    public bool RemoveQuest(string id)
    {
        if (IsExisting(id))
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

        if (IsExisting(id))
        {
            return false;
        }

        Quests[id] = quest;
        return true;
    }

    public bool ToComplete(string id)
    {
        if (IsExisting(id))
        {
            return false;
        }

        Quest quest = Quests[id];

        if (quest.IsCompleted)
        {
            throw new InvalidOperationException("Quest is completed");
        }

        quest.ToComplete();
        return true;
    }

    private bool IsExisting(string id)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }

        // Тут була помилка в логіці (ContatinsKey замість ContainsKey)
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