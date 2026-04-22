using Microsoft.EntityFrameworkCore;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Infrastructure.Database;

public class LifeQuestDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Quest> Quests { get; set; } = null!;

    public LifeQuestDbContext()
    {
    }

    public LifeQuestDbContext(DbContextOptions<LifeQuestDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=lifequest.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Quest>().HasKey(q => q.Id);
    }
}