using System;
using LifeQuest.Domain.Entities;
using NUnit.Framework;

namespace LifeQuest.Domain.Tests
{
    [TestFixture]
    public class UserTests
    {
        private User _user;

        [SetUp]
        public void Setup()
        {
            _user = new User(1, "Login", "Pass");
        }

        [Test]
        public void UpdateGold_IncreasesUserGold()
        {
            _user.UpdateGold(50);
            Assert.That(_user.Gold, Is.EqualTo(50));
        }

        [Test]
        public void UpdateExperience_IncreasesUserXP()
        {
            _user.UpdateExperience(100);
            Assert.That(_user.XP, Is.EqualTo(100));
        }

        [Test]
        public void IncreaseStreak_IncreasesStreakValue()
        {
            _user.IncreaseStreak();
            Assert.That(_user.Streak, Is.EqualTo(1));
        }

        [Test]
        public void ResetStreak_SetsStreakToZero()
        {
            _user.IncreaseStreak();
            _user.ResetStreak();
            Assert.That(_user.Streak, Is.EqualTo(0));
        }

        [Test]
        public void UpdateGold_AddsGoldToUserStats()
        {
            _user.UpdateGold(100);
            Assert.That(_user.UserStats.Gold, Is.EqualTo(100));
        }

        [Test]
        public void UpdateExperience_AddsExperienceToLevel()
        {
            _user.UpdateExperience(50);
            Assert.That(_user.UserStats.Level.CurrentExperience, Is.EqualTo(50));
        }
    }
}