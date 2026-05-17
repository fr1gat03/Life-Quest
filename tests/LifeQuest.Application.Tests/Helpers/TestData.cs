using LifeQuest.Domain.Components;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.Application.Tests.Helpers;

public static class TestData
{
    public static Quest NewQuest(bool isCompleted = false)
    {
        var quest = new Quest("Тестовий квест", rewardXp: 50, rewardGold: 10, Difficulty.Medium);
        if (isCompleted) quest.ToComplete();
        return quest;
    }

    public static User NewUser() => new(1, "hero", "hash");
}