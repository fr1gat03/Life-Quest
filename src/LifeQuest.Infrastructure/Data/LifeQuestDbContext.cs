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

            entity.OwnsOne(u => u.UserStats, stats =>
            {
                stats.OwnsOne(s => s.Level);
            });

                  .WithOne()
                  .HasForeignKey("UserId");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(q => q.Id);
        });
    }
}