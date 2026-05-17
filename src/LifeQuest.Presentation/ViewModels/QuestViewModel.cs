using System;
using System.Windows.Input;
using Avalonia.Media;

namespace LifeQuest.Presentation.ViewModels;

public class QuestViewModel : ViewModelBase
{
    private bool _isCompleted;

    public string Title { get; }
    public int RewardXp { get; }
    public int RewardGold { get; }
    public string Difficulty { get; }

    public string DifficultyLabel => Difficulty switch
    {
        "Easy"  => "🟢 Easy",
        "Hard"  => "🔴 Hard",
        "Epic"  => "🟣 Epic",
        _       => "🔵 Medium"
    };

    public IBrush DifficultyColor => Difficulty switch
    {
        "Easy"  => new SolidColorBrush(Color.Parse("#A6E3A1")),
        "Hard"  => new SolidColorBrush(Color.Parse("#F38BA8")),
        "Epic"  => new SolidColorBrush(Color.Parse("#CBA6F7")),
        _       => new SolidColorBrush(Color.Parse("#89B4FA"))
    };

    public bool IsCompleted
    {
        get => _isCompleted;
        set { _isCompleted = value; OnPropertyChanged(); }
    }

    public ICommand CompleteCommand { get; }

    public QuestViewModel(string title, int rewardXp, int rewardGold, string difficulty, Action<QuestViewModel> onComplete)
    {
        Title = title;
        RewardXp = rewardXp;
        RewardGold = rewardGold;
        Difficulty = difficulty;

        CompleteCommand = new RelayCommand(() =>
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                onComplete(this);
            }
        });
    }
}