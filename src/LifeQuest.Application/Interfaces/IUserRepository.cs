using LifeQuest.Domain.Entities;

namespace LifeQuest.Application.Interfaces;

public interface IUserRepository
{
    User? GetUserById(int id);
    User? GetUserByLogin(string login);
    void SaveUser(User user);
}