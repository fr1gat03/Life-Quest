using System.Collections.ObjectModel;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Presentation.ViewModels;

public class MainWindowViewModel : ViewModelBase
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

    public ObservableCollection<string> ActiveQuests { get; } = 
    [
        "Випити 2 літри водички (+14 XP)",
        "Прочитати документацію Avalonia (+88 XP)",
        "Зробити ррку з матаналізу (+67 XP)",
        "Смачно поїсти (+69 XP)"
    ];

    public MainWindowViewModel()
    {
        _user = new User("Sviat", "hashed_password_123");
        
        _user.UpdateExperience(20);
    }
}