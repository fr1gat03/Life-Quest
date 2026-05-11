using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Tests;

public class FakeUserRepository : IUserRepository
{
    public User? SavedUser { get; set; }

    public User GetUserById(int id)
    {
        return SavedUser!;
    }

    public void SaveUser(User user)
    {
        SavedUser = user;
    }
}