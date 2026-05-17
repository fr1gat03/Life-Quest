using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Tests.Fakes;

public sealed class FakeUserRepository : IUserRepository
{
    public User SavedUser { get; private set; }

    public User GetUserById(int id) => null;
    public void SaveUser(User user) => SavedUser = user;
}