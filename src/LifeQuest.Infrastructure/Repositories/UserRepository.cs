using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;
using LifeQuest.Infrastructure.Data;
using System.Linq;

namespace LifeQuest.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LifeQuestDbContext _context;

    public UserRepository(LifeQuestDbContext context)
    {
        _context = context;
    }

    public User? GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login);
    }

    public void SaveUser(User user)
    {
        if (!_context.Users.Any(u => u.Id == user.Id))
        {
            _context.Users.Add(user);
        }
        else
        {
            _context.Users.Update(user);
        }
        _context.SaveChanges();
    }
}