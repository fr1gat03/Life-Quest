using System;
using LifeQuest.Domain.Components;

namespace LifeQuest.Domain.Entities
{
    public class User
    {
        public UserStats UserStats { get; set; } = new UserStats();

        public int Gold => this.UserStats.Gold;
        public int XP => this.UserStats.Level.CurrentExperience;
        public int HealthPoints => this.UserStats.HealthPoints;
        public int Level => this.UserStats.Level.LevelValue;

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public QuestCollection Quests { get; private set; }
        public int Streak { get; private set; }

        public User(int id, string login, string passwordHash)
        {
            Id = id;
            Login = login ?? string.Empty;
            PasswordHash = passwordHash ?? string.Empty;
            Quests = new QuestCollection();
        }

        public User()
        {
            Quests = new QuestCollection();
        }

        public void UpdateHealth(int amount)
        {
            this.UserStats.UpdateHeatPoints(amount);
        }

        public void UpdateExperience(int amount)
        {
            this.UserStats.Level.UpdateExperience(amount);
        }

        public bool ToComplete(string id)
        {
            return Quests.ToComplete(id);
        }

        public void IncreaseStreak()
        {
            Streak++;
        }

        public void ResetStreak()
        {
            Streak = 0;
        }

        public void UpdateHealthPoints(int healthPoints)
        {
            int difference = healthPoints - this.UserStats.HealthPoints;
            this.UserStats.UpdateHeatPoints(difference);
        }

        public void UpdateGold(int gold)
        {
            this.UserStats.UpdateGold(gold);
        }
    }
}