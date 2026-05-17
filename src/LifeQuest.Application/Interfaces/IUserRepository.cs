using LifeQuest.Domain.Entities;
namespace LifeQuest.Application.Interfaces;

public interface IUserRepository
{
    User GetUserById(int id);
    void SaveUser(User user);
}