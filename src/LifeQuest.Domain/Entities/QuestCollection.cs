using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities;

public class QuestCollection
{
    private Dictionary<string, Quest> collection = new Dictionary<string, Quest>();

    public bool RemoveQuest(string id)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }

        collection.Remove(id);
        return true;
    }

    public bool AddQuest(string id, Quest quest)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }

        if (!IsValidQuest(quest))
        {
            throw new ArgumentException("Invalid quest");
        }

        collection[id] = quest;
        return true;
    }

    public bool ToComplete(string id)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Invalid id");
        }

        Quest quest = collection[id];

        if (quest.IsCompleted)
        {
            throw new InvalidOperationException("Quest is completed");
        }

        quest.ToComplete();
        return true;
    }

    private bool IsValidId(string id)
    {
        bool uniqueId = collection.ContainsKey(id);

        return uniqueId;
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