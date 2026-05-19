using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Tests.Fakes;

public class FakeUserRepository : IUserRepository
{
    public User? SavedUser { get; set; }

    public User? GetUserById(int id)
    {
        return SavedUser?.Id == id ? SavedUser : null;
    }

    public User? GetUserByLogin(string login)
    {
        return SavedUser?.Login == login ? SavedUser : null;
    }

    public void SaveUser(User user)
    {
        SavedUser = user;
    }
}