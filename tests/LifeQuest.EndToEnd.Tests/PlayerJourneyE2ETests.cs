using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using User = LifeQuest.Domain.Entities.User;

namespace LifeQuest.E2E.Tests
{
    public class PlayerJourneyE2ETests
    {
        [Test]
        public async Task NewPlayer_RegisterLoginCreateAndCompleteQuest_ProgressPersistedAcrossSessions()
        {
            var app = new TestApp();
            int userId;
            string questId;

            using (var session1 = app.NewSession())
            {
                userId = TestApp.NewId();
                var user = new User(userId, "user", string.Empty);
                user.SetPassword("Password9090!");
                session1.UserRepository.SaveUser(user);

                var quest = new Quest(Guid.NewGuid().ToString(),
                    "Виконати завдання", 80, 40, Difficulty.Medium, userId);

                session1.QuestRepository.UpdateQuest(quest);
                questId = quest.Id;
            }

            using (var session2 = app.NewSession())
            {
                var loggedIn = session2.UserRepository.GetUserByLogin("user");
                Assert.That(loggedIn, Is.Not.Null);
                Assert.That(loggedIn.VerifyPassword("Password9090!"), Is.True);

                var active = session2.QuestRepository.GetActiveQuests(userId).ToList();
                Assert.That(active.Count, Is.EqualTo(1));

                var result = await session2.QuestService.CompleteQuestAsync(active[0], loggedIn);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.MotivationMessage, Does.Contain("Виконати завдання"));
            }

            using (var session3 = app.NewSession())
            {
                var finalUser = session3.UserRepository.GetUserById(userId);
                Assert.That(finalUser.XP, Is.EqualTo(80));
                Assert.That(finalUser.Gold, Is.EqualTo(40));

                var completedQuest = session3.QuestRepository.GetQuestById(questId);
                Assert.That(completedQuest.IsCompleted, Is.True);

                Assert.That(session3.QuestRepository.GetActiveQuests(userId).Any(), Is.False);
                Assert.That(session3.QuestRepository.GetCompletedQuestsCount(userId), Is.EqualTo(1));
            }
        }
    }
}