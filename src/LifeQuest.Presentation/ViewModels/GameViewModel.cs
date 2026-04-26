using System.Collections.ObjectModel;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Presentation.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly User _user;

    public string PlayerName => _user.Login;
    public string PlayerLevel => $"Рівень {_user.UserStats.Level.LevelValue}";
    
    public int CurrentHp => _user.UserStats.HeatPoints;
    public int MaxHp => 100; 
    public string HpText => $"{CurrentHp}/{MaxHp}";
    public int CurrentXp => _user.UserStats.Level.CurrentExpirience;
    public int MaxXp => _user.UserStats.Level.MaxExpirience;
    public string XpText => $"{CurrentXp}/{MaxXp}";
    
    public int Gold => _user.UserStats.Gold;

    public ObservableCollection<QuestViewModel> ActiveQuests { get; }

    public GameViewModel(string username)
    {
        _user = new User(username, "dummy_password_hash");
        _user.UpdateExperience(20);

        ActiveQuests = new ObservableCollection<QuestViewModel>
        {
            new QuestViewModel("⚔️ Перше випробування: Привіт світ!", 50, 10, CompleteQuest),
            new QuestViewModel("📜 Прочитай 📝Tasks в тг каналі", 30, 5, CompleteQuest),
            new QuestViewModel("🧪 Протестувати нарахування досвіду 🤯", 100, 20, CompleteQuest)
        };
    }
    private void CompleteQuest(QuestViewModel quest)
    {
        _user.UpdateExperience(quest.RewardXp);
        _user.UpdateGold(quest.RewardGold);

        OnPropertyChanged(nameof(CurrentXp));
        OnPropertyChanged(nameof(MaxXp));
        OnPropertyChanged(nameof(XpText));
        OnPropertyChanged(nameof(PlayerLevel));
        OnPropertyChanged(nameof(Gold));
        
        // Можна видалити квест зі списку після виконання
        // ActiveQuests.Remove(quest); 
    }
}