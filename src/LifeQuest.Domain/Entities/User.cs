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
        
        public string Avatar { get; private set; } = "⚔️";

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
        
        public void UpdateLogin(string newLogin)
        {
            if (!string.IsNullOrEmpty(newLogin))
                Login = newLogin;
        }
        
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(PasswordHash)) return false;
    
            var parts = PasswordHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }
    
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);
            byte[] actualHash = LifeQuest.Domain.SecureData.PasswordHasher.HashPassword(password, salt);
    
            return expectedHash.SequenceEqual(actualHash);
        }

        public void SetPassword(string password)
        {
            byte[] salt = LifeQuest.Domain.SecureData.PasswordHasher.GenerateSalt(16);
            byte[] hash = LifeQuest.Domain.SecureData.PasswordHasher.HashPassword(password, salt);
            PasswordHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
        
        public void UpdateAvatar(string avatar)
        {
            if (!string.IsNullOrEmpty(avatar))
                Avatar = avatar;
        }
    }
}