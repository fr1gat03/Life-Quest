using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
            var aiService = new GeminiAiService();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(aiService),
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}