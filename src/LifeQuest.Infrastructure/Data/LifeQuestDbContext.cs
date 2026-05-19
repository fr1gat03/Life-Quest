using Microsoft.EntityFrameworkCore;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Infrastructure.Data;

public class LifeQuestDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Quest> Quests { get; set; }

    public LifeQuestDbContext(DbContextOptions<LifeQuestDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Ignore(u => u.Quests);

            entity.OwnsOne(u => u.UserStats, stats =>
            {
                stats.Property(s => s.HealthPoints);
                stats.Property(s => s.Gold);

                stats.OwnsOne(s => s.Level, level =>
                {
                    level.Property(l => l.LevelValue);
                    level.Property(l => l.CurrentExperience);
                    level.Property(l => l.MaxExperience);
                });
            });
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.Property(q => q.UserId);
            entity.Property(q => q.Title);
            entity.Property(q => q.RewardXp);
            entity.Property(q => q.RewardGold);
            entity.Property(q => q.IsCompleted);
            entity.Property(q => q.Difficulty);
        });
    }
}