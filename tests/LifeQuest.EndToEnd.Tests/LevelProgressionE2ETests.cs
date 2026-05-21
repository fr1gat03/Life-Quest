using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.E2E.Tests
{
    public class LevelProgressionE2ETests
    {
        [Test]
        public async Task PlayerCompletesSeveralQuests_LevelsUp_AndProgressSurvivesRestart()
        {
            var app = new TestApp();
            var userId = TestApp.NewId();

            using (var session1 = app.NewSession())
            {
                var user = new User(userId, "user", string.Empty);
                user.SetPassword("Password1211!");
                session1.UserRepository.SaveUser(user);

                for (var i = 0; i < 3; i++)
                {
                    var quest = new Quest(Guid.NewGuid().ToString(),
                        "Завдання" + i, 100, 10, Difficulty.Hard, userId);

                    session1.QuestRepository.UpdateQuest(quest);

                    var fresh = session1.UserRepository.GetUserById(userId);
                    var result = await session1.QuestService.CompleteQuestAsync(quest, fresh);
                    Assert.That(result.IsSuccess, Is.True);
                }
            }

            using (var session2 = app.NewSession())
            {
                var user = session2.UserRepository.GetUserById(userId);

                Assert.That(user.Level, Is.EqualTo(2));
                Assert.That(user.XP, Is.EqualTo(150));
                Assert.That(session2.QuestRepository.GetCompletedQuestsCount(userId), Is.EqualTo(3));
            }
        }

        [Test]
        public async Task SingleSmallQuest_DoesNotLevelUp_StaysLevelOne()
        {
            var app = new TestApp();
            var userId = TestApp.NewId();

            using (var session1 = app.NewSession())
            {
                var user = new User(userId, "user", string.Empty);
                user.SetPassword("Password123!");
                session1.UserRepository.SaveUser(user);

                var quest = new Quest(Guid.NewGuid().ToString(),
                    "Інше завдання", 30, 5, Difficulty.Easy, userId);

                session1.QuestRepository.UpdateQuest(quest);

                await session1.QuestService.CompleteQuestAsync(quest, user);
            }

            using (var session2 = app.NewSession())
            {
                var user = session2.UserRepository.GetUserById(userId);
                Assert.That(user.Level, Is.EqualTo(1));
                Assert.That(user.XP, Is.EqualTo(30));
            }
        }
    }
}