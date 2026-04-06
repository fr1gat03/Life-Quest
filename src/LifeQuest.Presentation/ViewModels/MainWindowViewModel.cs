using System.Collections.ObjectModel;

namespace LifeQuest.Presentation.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string PlayerName { get; } = "Розробник";
    public string PlayerLevel { get; } = "Рівень 1 (Новачок)";
    
    public int CurrentHp { get; } = 80;
    public int MaxHp { get; } = 100;
    public string HpText => $"{CurrentHp}/{MaxHp}";
    
    public int CurrentXp { get; } = 45;
    public int MaxXp { get; } = 100;
    public string XpText => $"{CurrentXp}/{MaxXp}";
    
    public int Gold { get; } = 52;

    public ObservableCollection<string> ActiveQuests { get; } = 
    [
            "Випити 2 літри водички (+14 XP)",
            "Прочитати документацію Avalonia (+88 XP)",
            "Зробити ррку з матаналізу (+67 XP)",
            "Смачно поїсти (+69 XP)"
    ];
}