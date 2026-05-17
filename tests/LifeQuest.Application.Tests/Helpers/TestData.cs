using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.Application.Tests;

public static class TestData
{
    public static Quest NewQuest()
    {
        return new Quest("1", "Тестовий квест", 100, 50, Difficulty.Medium);
    }

    public static User NewUser()
    {
        return new User(1, "TestUser", "hash");
    }
}