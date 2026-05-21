using LifeQuest.Application.Interfaces;
using LifeQuest.Application.Services;
using LifeQuest.Infrastructure.Data;
using LifeQuest.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.E2E.Tests
{
    public sealed class AppSession : IDisposable
    {
        public LifeQuestDbContext DataBase { get; }
        public UserRepository UserRepository { get; }
        public QuestRepository QuestRepository { get; }
        public IAiService Ai { get; }
        public QuestService QuestService { get; }

        public AppSession(string dbPath)
        {
            var options = new DbContextOptionsBuilder<LifeQuestDbContext>()
                .UseSqlite("data source =" + dbPath).Options;

            DataBase = new LifeQuestDbContext(options);
            DataBase.Database.EnsureCreated();

            UserRepository = new UserRepository(DataBase);
            QuestRepository = new QuestRepository(DataBase);
            Ai = new FakeAiService();
            QuestService = new QuestService(Ai, UserRepository, QuestRepository);
        }

        public void Dispose() => DataBase.Dispose();
    }

    public sealed class TestApp : IDisposable
    {
        private static int _idCounter = 10000;
        private readonly string _dbPath;
        public static int NewId() => Interlocked.Increment(ref _idCounter);

        public TestApp()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), "e2e" + Guid.NewGuid() + ".db");
        }

        public AppSession NewSession() => new AppSession(_dbPath);

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();

            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }
    }
}