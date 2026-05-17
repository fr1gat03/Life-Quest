using System;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace LifeQuest.Presentation.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly Action _onBack;

    private string _username = "";
    private string _apiKey = "";
    private string _selectedAvatar = "⚔️";
    private string _statusMessage = "";

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string ApiKey
    {
        get => _apiKey;
        set { _apiKey = value; OnPropertyChanged(); }
    }

    public string SelectedAvatar
    {
        get => _selectedAvatar;
        set { _selectedAvatar = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public int HeroId { get; }

    public ObservableCollection<string> Avatars { get; } = new()
    {
        "⚔️", "🧙", "🏹", "🛡️", "🐉", "🧝", "🦸", "🔮", "👑", "💀"
    };

    public ICommand SaveCommand { get; }
    public ICommand ResetProgressCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand SelectAvatarCommand { get; }

    public SettingsViewModel(int heroId, string username, string currentApiKey, Action onBack)
    {
        HeroId = heroId;
        _username = username;
        _apiKey = currentApiKey;
        _onBack = onBack;

        BackCommand = new RelayCommand(() => _onBack());

        SaveCommand = new RelayCommand(() =>
        {
            // TODO: зберегти в БД коли буде Infrastructure
            StatusMessage = "✅ Зміни збережено!";
        });

        ResetProgressCommand = new RelayCommand(() =>
        {
            StatusMessage = "⚠️ Прогрес скинуто (TODO: підключити до БД)";
        });

        SelectAvatarCommand = new RelayCommand(() => { });
    }
}