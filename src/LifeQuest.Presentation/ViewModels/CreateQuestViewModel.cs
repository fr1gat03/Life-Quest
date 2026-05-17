using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Presentation.ViewModels;

public class CreateQuestViewModel : ViewModelBase
{
    private readonly IAiService _aiService;
    private readonly Action<AiQuestProposal> _onQuestCreated;
    private readonly Action _onCancel;

    private string _userInput = "";
    private string _title = "";
    private string _difficulty = "Medium";
    private int _rewardXp = 0;
    private int _rewardGold = 0;
    private bool _isLoading = false;
    private string _errorMessage = "";

    // Властивості для прив'язки до UI
    public string UserInput { get => _userInput; set { _userInput = value; OnPropertyChanged(); } }
    public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
    public string Difficulty { get => _difficulty; set { _difficulty = value; OnPropertyChanged(); } }
    public int RewardXp { get => _rewardXp; set { _rewardXp = value; OnPropertyChanged(); } }
    public int RewardGold { get => _rewardGold; set { _rewardGold = value; OnPropertyChanged(); } }
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

    public ICommand GenerateCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public CreateQuestViewModel(IAiService aiService, Action<AiQuestProposal> onQuestCreated, Action onCancel)
    {
        _aiService = aiService;
        _onQuestCreated = onQuestCreated;
        _onCancel = onCancel;

        GenerateCommand = new RelayCommand(async () => await GenerateWithAi());
        SaveCommand = new RelayCommand(async () => await ValidateAndSave());
        CancelCommand = new RelayCommand(() => _onCancel());
    }

    private async Task GenerateWithAi()
    {
        if (string.IsNullOrWhiteSpace(UserInput)) return;

        IsLoading = true;
        ErrorMessage = "✨ ШІ думає...";

        var proposal = await _aiService.AnalyzeAndBalanceQuest(UserInput);
        
        Title = proposal.Title;
        Difficulty = proposal.Difficulty;
        RewardXp = proposal.RewardXp;
        RewardGold = proposal.RewardGold;

        ErrorMessage = "";
        IsLoading = false;
    }

    private async Task ValidateAndSave()
    {
        IsLoading = true;
        ErrorMessage = "⚖️ Гейм-майстер перевіряє баланс...";

        var currentProposal = new AiQuestProposal 
        { 
            Title = Title, Difficulty = Difficulty, 
            RewardXp = RewardXp, RewardGold = RewardGold 
        };

        var verdict = await _aiService.ValidateQuestFairness(currentProposal);

        if (verdict.IsFair)
        {
            _onQuestCreated(currentProposal); // Зберігаємо і закриваємо
        }
        else
        {
            ErrorMessage = $"❌ Чітерство! {verdict.Feedback}";
            IsLoading = false;
        }
    }
}