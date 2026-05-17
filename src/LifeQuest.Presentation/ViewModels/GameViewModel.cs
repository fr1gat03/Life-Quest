using System.Collections.ObjectModel;
using System.Windows.Input;
using LifeQuest.Domain.Entities;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Presentation.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly User _user;
    private readonly IAiService _aiService;
    private readonly MainViewModel _mainNavigator;

    
    // Властивості для UI
    
    public string PlayerName => _user.Login;
    public string PlayerLevel => $"Рівень {_user.UserStats.Level.LevelValue}";
    public string AvatarText => _user.Login.Length >= 2
        ? _user.Login[..2].ToUpper()
        : _user.Login.ToUpper();

    public int CurrentHp => _user.UserStats.HealthPoints;
    public int MaxHp => 100;
    public string HpText => $"{CurrentHp}/{MaxHp}";

    public int CurrentXp => _user.UserStats.Level.CurrentExperience;
    public int MaxXp => _user.UserStats.Level.MaxExperience;
    public string XpText => $"{CurrentXp}/{MaxXp}";

    public int Gold => _user.UserStats.Gold;

    // Колекції та команди
    public ObservableCollection<QuestViewModel> ActiveQuests { get; }
    public ICommand OpenCreateQuestCommand { get; }
    public ICommand OpenTavernCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    // Єдиний правильний конструктор на 3 параметри!
    public GameViewModel(int id, string username, IAiService aiService, MainViewModel mainNavigator)
    {
        _user = new User(id, username, "dummy_password_hash");
        _user.UpdateExperience(20);

        _aiService = aiService;
        _mainNavigator = mainNavigator;

        ActiveQuests = new ObservableCollection<QuestViewModel>
        {
            new QuestViewModel("⚔️ Перше випробування: Привіт світ!", 50, 10, "Medium", CompleteQuest),
            new QuestViewModel("📜 Прочитай 📝Tasks в тг каналі", 30, 5, "Easy", CompleteQuest),
            new QuestViewModel("🧪 Протестувати нарахування досвіду 🤯", 100, 20, "Hard", CompleteQuest)
        };

        OpenCreateQuestCommand = new RelayCommand(() => _mainNavigator.NavigateToCreateQuest(this));
        OpenTavernCommand = new RelayCommand(() => _mainNavigator.NavigateToTavern(this));
        OpenSettingsCommand = new RelayCommand(() => _mainNavigator.NavigateToSettings(this));
    }

    // Метод для додавання згенерованого ШІ квесту
    public void AddQuestFromAi(AiQuestProposal proposal)
    {
        var newQuest = new QuestViewModel(
            proposal.Title,
            proposal.RewardXp,
            proposal.RewardGold,
            proposal.Difficulty,
            CompleteQuest
        );
        ActiveQuests.Insert(0, newQuest);
    }

    // Метод виконання квесту
    private void CompleteQuest(QuestViewModel quest)
    {
        _user.UpdateExperience(quest.RewardXp);
        _user.UpdateGold(quest.RewardGold);

        OnPropertyChanged(nameof(CurrentXp));
        OnPropertyChanged(nameof(MaxXp));
        OnPropertyChanged(nameof(XpText));
        OnPropertyChanged(nameof(PlayerLevel));
        OnPropertyChanged(nameof(Gold));
    }
} 