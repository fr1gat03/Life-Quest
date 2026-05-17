using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LifeQuest.Infrastructure.Services;
using LifeQuest.Infrastructure.Data;
using LifeQuest.Infrastructure.Repositories;
using LifeQuest.Presentation.ViewModels;
using LifeQuest.Presentation.Views;
using System.IO;
using System;

namespace LifeQuest.Presentation;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var apiKey = config["GeminiApiKey"]
                ?? throw new InvalidOperationException("GeminiApiKey not found in appsettings.json");

            var aiService = new GeminiAiService(apiKey);

           
            string dbPath = Path.Combine(AppContext.BaseDirectory, "lifequest.db");
            var optionsBuilder = new DbContextOptionsBuilder<LifeQuestDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            var dbContext = new LifeQuestDbContext(optionsBuilder.Options);

           
            dbContext.Database.EnsureCreated();

            var userRepository = new UserRepository(dbContext);
            var questRepository = new QuestRepository(dbContext);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(aiService, userRepository, questRepository),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}