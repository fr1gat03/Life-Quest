using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Presentation.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly Action _onBack;
    private readonly Action<string> _onUsernameSaved;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;
    private readonly Action<string> _onApiKeySaved;
    private readonly Action _onProgressReset;
    
    public string AppVersion => 
        System.Reflection.Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString(3) ?? "0.0.1";


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

    public SettingsViewModel(int heroId, string username, string currentApiKey,
        IUserRepository userRepository, IQuestRepository questRepository, Action<string> onUsernameSaved, Action<string> onApiKeySaved, Action onProgressReset, Action onBack)
    {
        HeroId = heroId;
        _username = username;
        _apiKey = currentApiKey;
        _userRepository = userRepository;
        _onUsernameSaved = onUsernameSaved;
        _onBack = onBack;
        _questRepository = questRepository;
        _onProgressReset = onProgressReset;

        BackCommand = new RelayCommand(() => _onBack());
        SelectAvatarCommand = new RelayCommand(() => { });
        
        _onApiKeySaved = onApiKeySaved;

        SaveCommand = new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                StatusMessage = "⚠️ Ім'я не може бути порожнім";
                return;
            }

            var existingUser = _userRepository.GetUserByLogin(Username);
            if (existingUser != null && existingUser.Id != HeroId)
            {
                StatusMessage = "❌ Цей логін вже використовується іншим героєм";
                return;
            }

            var user = _userRepository.GetUserById(HeroId);
            if (user != null)
            {
                user.UpdateLogin(Username);
                user.UpdateAvatar(SelectedAvatar);
                _userRepository.SaveUser(user);
            }

            if (!string.IsNullOrWhiteSpace(ApiKey))
                _onApiKeySaved(ApiKey);

            StatusMessage = "✅ Зміни збережено!";
            _onUsernameSaved(Username);
        });
        
        ResetProgressCommand = new RelayCommand(() =>
        {
            var user = _userRepository.GetUserById(HeroId);
            if (user != null)
            {
                user.ResetProgress();
                _userRepository.SaveUser(user);
                _questRepository.DeleteAllUserQuests(HeroId);
                StatusMessage = "⚔️ Прогрес скинуто. Починай знову, герою!";
                _onProgressReset();
            }
        });
    }
}