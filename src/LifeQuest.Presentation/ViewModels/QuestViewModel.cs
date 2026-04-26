using System;
using System.Windows.Input;

namespace LifeQuest.Presentation.ViewModels;

public class QuestViewModel : ViewModelBase
{
    private bool _isCompleted;

    public string Title { get; }
    public int RewardXp { get; }
    public int RewardGold { get; }

    public bool IsCompleted
    {
        get => _isCompleted;
        set { _isCompleted = value; OnPropertyChanged(); }
    }

    public ICommand CompleteCommand { get; }

    public QuestViewModel(string title, int rewardXp, int rewardGold, Action<QuestViewModel> onComplete)
    {
        Title = title;
        RewardXp = rewardXp;
        RewardGold = rewardGold;
        
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