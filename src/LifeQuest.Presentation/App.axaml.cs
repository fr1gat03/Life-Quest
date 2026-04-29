using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using LifeQuest.Infrastructure.Services;
using LifeQuest.Presentation.ViewModels;
using LifeQuest.Presentation.Views;

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

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(aiService),
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}