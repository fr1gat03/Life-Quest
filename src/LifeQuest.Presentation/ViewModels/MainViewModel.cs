using System;
using System.IO;
using System.Text.Json;
using LifeQuest.Application.Interfaces;
using LifeQuest.Application.Services;

namespace LifeQuest.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;
    private readonly IAiService _aiService;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;
    private readonly QuestService _questService;

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set { _currentPage = value; OnPropertyChanged(); }
    }

    public MainViewModel(IAiService aiService, IUserRepository userRepository, IQuestRepository questRepository)
    {
        _aiService = aiService;
        _userRepository = userRepository;
        _questRepository = questRepository;

        _questService = new QuestService(_aiService, _userRepository, _questRepository);

        _currentPage = new LoginViewModel(this, _userRepository);
    }

    public void NavigateToGame(int id, string username)
    {
        CurrentPage = new GameViewModel(
            id, username, _questService,
            _userRepository, _questRepository, this
        );
    }

    public void NavigateToCreateQuest(GameViewModel gameVm)
    {
        CurrentPage = new CreateQuestViewModel(
            _aiService,
            proposal => {
                gameVm.AddQuestFromAi(proposal);
                NavigateBackToGame(gameVm);
            },
            () => NavigateBackToGame(gameVm)
        );
    }

    public void NavigateToSettings(GameViewModel gameVm)
    {
        CurrentPage = new SettingsViewModel(
            gameVm.UserId,
            gameVm.PlayerName,
            GetCurrentApiKey(),
            _userRepository,
            _questRepository,
            (newUsername) => {
                gameVm.RefreshAfterSettings();
                NavigateBackToGame(gameVm);
            },
            (newApiKey) => SaveApiKey(newApiKey),
            () => {
                gameVm.ResetAndReload();
                NavigateBackToGame(gameVm);
            },
            () => NavigateBackToGame(gameVm)
        );
    }
    
    private void SaveApiKey(string apiKey)
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            var json = JsonSerializer.Serialize(new { GeminiApiKey = apiKey },
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
            Console.WriteLine("[Settings] API ключ збережено");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Settings] Помилка збереження API ключа: {ex.Message}");
        }
    }
    
    private string GetCurrentApiKey()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            var json = File.ReadAllText(path);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("GeminiApiKey").GetString() ?? "";
        }
        catch
        {
            return ""; 
        }
    }

    private TavernViewModel? _cachedTavern;
    private GameViewModel? _cachedTavernOwner;

    public void NavigateToTavern(GameViewModel gameVm)
    {
        if (_cachedTavern == null || _cachedTavernOwner != gameVm)
        {
            _cachedTavern = new TavernViewModel(
                _aiService,
                () => NavigateBackToGame(gameVm),
                (npcAdvice) => {
                    var createQuestVm = new CreateQuestViewModel(
                        _aiService,
                        proposal => {
                            gameVm.AddQuestFromAi(proposal);
                            NavigateBackToGame(gameVm);
                        },
                        () => NavigateBackToGame(gameVm)
                    );
                    createQuestVm.UserInput = npcAdvice;
                    CurrentPage = createQuestVm;
                }
            );
            _cachedTavernOwner = gameVm;
        }
        else
        {
            _cachedTavern.UpdateBackCallback(() => NavigateBackToGame(gameVm));
        }

        CurrentPage = _cachedTavern;
    }

    private void NavigateBackToGame(GameViewModel gameVm)
    {
        CurrentPage = gameVm;
    }
}