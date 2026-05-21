using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LifeQuest.Domain.Entities;
using LifeQuest.Domain.Enums;
using LifeQuest.Application.Interfaces;
using LifeQuest.Application.Services;

namespace LifeQuest.Presentation.ViewModels;

public class GameViewModel : ViewModelBase
{
    private User _user;
    private readonly QuestService _questService;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;
    private readonly MainViewModel _mainNavigator;

    private string _motivationMessage = "";

    public string PlayerName => _user.Login;
    public string PlayerLevel => $"Рівень {_user.UserStats.Level.LevelValue}";

    public string AvatarDisplay => string.IsNullOrEmpty(_user.Avatar) || _user.Avatar == "⚔️"
        ? (_user.Login.Length >= 2 ? _user.Login[..2].ToUpper() : _user.Login.ToUpper())
        : _user.Avatar;

    public bool AvatarIsEmoji => !string.IsNullOrEmpty(_user.Avatar) && _user.Avatar != "⚔️";
    public int UserId => _user.Id;

    public int CurrentHp => _user.UserStats.HealthPoints;
    public int MaxHp => 100;
    public string HpText => $"{CurrentHp}/{MaxHp}";

    public int CurrentXp => _user.UserStats.Level.CurrentExperience;
    public int MaxXp => _user.UserStats.Level.MaxExperience;
    public string XpText => $"{CurrentXp}/{MaxXp}";

    public int Gold => _user.UserStats.Gold;
    
    public string StreakText => _user.Streak > 0 ? $"🔥 {_user.Streak} день поспіль" : "";
    public bool HasStreak => _user.Streak > 0;

    public string MotivationMessage
    {
        get => _motivationMessage;
        set { _motivationMessage = value; OnPropertyChanged(); }
    }

    public bool HasMotivation => !string.IsNullOrEmpty(_motivationMessage);

    public ObservableCollection<QuestViewModel> ActiveQuests { get; } = new();
    public ICommand OpenCreateQuestCommand { get; }
    public ICommand OpenTavernCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public GameViewModel(int id, string username, QuestService questService,
        IUserRepository userRepository, IQuestRepository questRepository,
        MainViewModel mainNavigator)
    {
        _questService = questService;
        _userRepository = userRepository;
        _questRepository = questRepository;
        _mainNavigator = mainNavigator;

        var existingUser = _userRepository.GetUserById(id);
        _user = existingUser ?? new User(id, username, "");
        if (existingUser == null)
            _userRepository.SaveUser(_user);

        LoadQuestsFromDb();
        if (!_questRepository.HasAnyQuests(_user.Id))
            AddDefaultQuests();

        OpenCreateQuestCommand = new RelayCommand(() => _mainNavigator.NavigateToCreateQuest(this));
        OpenTavernCommand = new RelayCommand(() => _mainNavigator.NavigateToTavern(this));
        OpenSettingsCommand = new RelayCommand(() => _mainNavigator.NavigateToSettings(this));
    }

    private void LoadQuestsFromDb()
    {
        var quests = _questRepository.GetActiveQuests(_user.Id);
        foreach (var quest in quests)
        {
            ActiveQuests.Add(new QuestViewModel(
                quest.Id,
                quest.Title,
                quest.RewardXp,
                quest.RewardGold,
                quest.Difficulty.ToString(),
                CompleteQuest
            ));
        }
    }

    private void AddDefaultQuests()
    {
        var defaultQuests = new[]
        {
            new Quest(Guid.NewGuid().ToString(), "⚔️ Перше випробування: Привіт світ!", 50, 10, Difficulty.Medium, _user.Id),
            new Quest(Guid.NewGuid().ToString(), "📜 Прочитай Tasks в тг каналі", 30, 5, Difficulty.Easy, _user.Id),
            new Quest(Guid.NewGuid().ToString(), "🧪 Протестувати нарахування досвіду", 100, 20, Difficulty.Hard, _user.Id)
        };

        foreach (var quest in defaultQuests)
        {
            _questRepository.UpdateQuest(quest);
            ActiveQuests.Add(new QuestViewModel(
                quest.Id,
                quest.Title,
                quest.RewardXp,
                quest.RewardGold,
                quest.Difficulty.ToString(),
                CompleteQuest
            ));
        }
    }

    public void AddQuestFromAi(AiQuestProposal proposal)
    {
        var difficulty = Enum.TryParse<Difficulty>(proposal.Difficulty, out var d) ? d : Difficulty.Medium;
        var questId = Guid.NewGuid().ToString();
        var quest = new Quest(questId, proposal.Title, proposal.RewardXp,
            proposal.RewardGold, difficulty, _user.Id);

        _questRepository.UpdateQuest(quest);
        ActiveQuests.Insert(0, new QuestViewModel(
            questId, quest.Title, quest.RewardXp,
            quest.RewardGold, proposal.Difficulty, CompleteQuest
        ));
    }

    private async Task CompleteQuest(QuestViewModel questVm)
    {
        var quest = _questRepository.GetQuestById(questVm.QuestId);
        if (quest == null) return;

        var result = await _questService.CompleteQuestAsync(quest, _user);

        if (result.IsSuccess)
        {
            var updatedUser = _userRepository.GetUserById(_user.Id);
            if (updatedUser != null)
            {
                _user = updatedUser;
                _user.UpdateStreak();
                _userRepository.SaveUser(_user);
            }

            if (!string.IsNullOrEmpty(result.MotivationMessage))
            {
                MotivationMessage = $"✨ {result.MotivationMessage}";
                OnPropertyChanged(nameof(HasMotivation));
            }

            OnPropertyChanged(nameof(CurrentXp));
            OnPropertyChanged(nameof(MaxXp));
            OnPropertyChanged(nameof(XpText));
            OnPropertyChanged(nameof(PlayerLevel));
            OnPropertyChanged(nameof(Gold));
            OnPropertyChanged(nameof(StreakText));  // ← додати
            OnPropertyChanged(nameof(HasStreak));
        }
    }
    
    public void ResetAndReload()
    {
        ActiveQuests.Clear();

        var updatedUser = _userRepository.GetUserById(_user.Id);
        if (updatedUser != null)
        {
            _user = updatedUser;
        }

        LoadQuestsFromDb();
        if (!_questRepository.HasAnyQuests(_user.Id))
        {
            AddDefaultQuests();
        }

        OnPropertyChanged(nameof(CurrentXp));
        OnPropertyChanged(nameof(MaxXp));
        OnPropertyChanged(nameof(XpText));
        OnPropertyChanged(nameof(PlayerLevel));
        OnPropertyChanged(nameof(Gold));
        OnPropertyChanged(nameof(CurrentHp));
        OnPropertyChanged(nameof(HpText));
        OnPropertyChanged(nameof(StreakText));
        OnPropertyChanged(nameof(HasStreak));
        OnPropertyChanged(nameof(AvatarDisplay));

        MotivationMessage = "";
        OnPropertyChanged(nameof(HasMotivation));
    }

    public void RefreshAfterSettings()
    {
        var updatedUser = _userRepository.GetUserById(_user.Id);
        if (updatedUser != null)
        {
            _user = updatedUser;
            OnPropertyChanged(nameof(PlayerName));
            OnPropertyChanged(nameof(AvatarDisplay));
            OnPropertyChanged(nameof(AvatarIsEmoji));
        }
    }
}