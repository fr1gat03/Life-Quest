using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class QuestCollection
{
    private Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

    public bool RemoveQuest(string id)
    {
        if (IsNewId(id))
        {
            return false;
        }

        quests.Remove(id);
        return true;
    }

    public bool AddQuest(string id, Quest quest)
    {
        if (!IsValidQuest(quest))
        {
            throw new ArgumentException("Invalid quest");
        }

        if (!IsNewId(id))
        {
            return false;
        }

        quests[id] = quest;
        return true;
    }

    public bool ToComplete(string id)
    {
        if (IsNewId(id))
        {
            return false;
        }

        Quest quest = quests[id];

        if (quest.IsCompleted)
        {
            throw new InvalidOperationException("Quest is completed");
        }

        quest.ToComplete();
        return true;
    }

    private bool IsNewId(string id)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }

        if (quests.ContainsKey(id))
        {
            return false;
        }

        return true;
    }

    private bool IsValidId(string id)
    {
        if (id == null)
        {
            return false;
        }

        return true;
    }

    private bool IsValidQuest(Quest quest)
    {
        bool correctQuest = true;

        if (quest.Title == null || quest.Title == "")
        {
            correctQuest = false;
        }

        return correctQuest;
    }
} 