using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;

namespace LifeQuest.E2E.Tests
{
    public class ResetProgressE2ETests
    {
        [Test]
        public async Task PlayerWithProgress_ResetsEverything_CleanStateSurvivesRestart()
        {
            var app = new TestApp();
            var userId = TestApp.NewId();

            using (var session1 = app.NewSession())
            {
                var user = new User(userId, "user", string.Empty);
                user.SetPassword("Password1010!");
                session1.UserRepository.SaveUser(user);

                var quest = new Quest(Guid.NewGuid().ToString(),
                    "Виконати завдання", 100, 50, Difficulty.Hard, userId);

                session1.QuestRepository.UpdateQuest(quest);

                var result = await session1.QuestService.CompleteQuestAsync(quest, user);
                Assert.That(result.IsSuccess, Is.True);
            }

            using (var session2 = app.NewSession())
            {
                var user = session2.UserRepository.GetUserById(userId);
                Assert.That(user.XP, Is.EqualTo(100));
                Assert.That(session2.QuestRepository.GetCompletedQuestsCount(userId), Is.EqualTo(1));

                user.ResetProgress();
                session2.UserRepository.SaveUser(user);
                session2.QuestRepository.DeleteAllUserQuests(userId);
            }

            using (var session3 = app.NewSession())
            {
                var user = session3.UserRepository.GetUserById(userId);

                Assert.That(user, Is.Not.Null);
                Assert.That(user.XP, Is.EqualTo(0));
                Assert.That(user.Gold, Is.EqualTo(0));
                Assert.That(user.Level, Is.EqualTo(1));
                Assert.That(user.Streak, Is.EqualTo(0));

                Assert.That(session3.QuestRepository.HasAnyQuests(userId), Is.False);
                Assert.That(session3.QuestRepository.GetCompletedQuestsCount(userId), Is.EqualTo(0));
            }
        }

        [Test]
        public void DeletingOnePlayersQuests_DoesNotAffectAnother_AcrossRestart()
        {
            var app = new TestApp();
            var user1Id = TestApp.NewId();
            var user2Id = TestApp.NewId();

            using (var session1 = app.NewSession())
            {
                var user1 = new User(user1Id, "user1", string.Empty);
                user1.SetPassword("Password1010!");
                session1.UserRepository.SaveUser(user1);

                var user2 = new User(user2Id, "user2", string.Empty);
                user2.SetPassword("Password1010!");
                session1.UserRepository.SaveUser(user2);

                session1.QuestRepository.UpdateQuest(new Quest(Guid.NewGuid().ToString(), "A1", 50, 20, Difficulty.Easy, user1Id));
                session1.QuestRepository.UpdateQuest(new Quest(Guid.NewGuid().ToString(), "A2", 50, 20, Difficulty.Easy, user1Id));
                session1.QuestRepository.UpdateQuest(new Quest(Guid.NewGuid().ToString(), "B1", 50, 20, Difficulty.Easy, user2Id));

                session1.QuestRepository.DeleteAllUserQuests(user1Id);
            }

            using (var session2 = app.NewSession())
            {
                Assert.That(session2.QuestRepository.HasAnyQuests(user1Id), Is.False);
                Assert.That(session2.QuestRepository.GetActiveQuests(user2Id).Count(), Is.EqualTo(1));
            }
        }
    }
}