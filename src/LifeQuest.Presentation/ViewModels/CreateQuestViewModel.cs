using System;
using System.Collections.Generic;
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

    private bool _isAiGenerated = false;
    private int _aiRewardXp = 0;
    private int _aiRewardGold = 0;
    private string _aiDifficulty = "";
    
    private AiQuestProposal? _lastAiProposal;

    public List<string> DifficultyOptions { get; } = new()
    {
        "Easy", "Medium", "Hard", "Epic"
    };

    public string UserInput
    {
        get => _userInput;
        set { _userInput = value; OnPropertyChanged(); }
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnPropertyChanged();
            if (_isAiGenerated && value != _aiDifficulty)
                _isAiGenerated = false;
        }
    }

    public int RewardXp
    {
        get => _rewardXp;
        set
        {
            _rewardXp = value;
            OnPropertyChanged();
            if (_isAiGenerated && value != _aiRewardXp)
                _isAiGenerated = false;
        }
    }

    public int RewardGold
    {
        get => _rewardGold;
        set
        {
            _rewardGold = value;
            OnPropertyChanged();
            if (_isAiGenerated && value != _aiRewardGold)
                _isAiGenerated = false;
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand GenerateCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public CreateQuestViewModel(IAiService aiService,
        Action<AiQuestProposal> onQuestCreated, Action onCancel)
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
        ErrorMessage = "🔮 Маг аналізує задачу...";

        var proposal = await _aiService.AnalyzeAndBalanceQuest(UserInput);

        Title = proposal.Title;
        Difficulty = proposal.Difficulty;
        RewardXp = proposal.RewardXp;
        RewardGold = proposal.RewardGold;

        _lastAiProposal = proposal;

        ErrorMessage = "✅ Маг визначив баланс!";
        IsLoading = false;
    }

    private async Task ValidateAndSave()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "⚠️ Введіть назву звиту";
            return;
        }

        var currentProposal = new AiQuestProposal
        {
            Title = Title,
            Difficulty = Difficulty,
            RewardXp = RewardXp,
            RewardGold = RewardGold
        };

        bool isUnchangedFromAi = _lastAiProposal != null
                                 && RewardXp == _lastAiProposal.RewardXp
                                 && RewardGold == _lastAiProposal.RewardGold
                                 && Difficulty == _lastAiProposal.Difficulty;

        if (isUnchangedFromAi)
        {
            ErrorMessage = "";
            _onQuestCreated(currentProposal);
            return;
        }

        IsLoading = true;
        ErrorMessage = "⚖️ Гейм-майстер перевіряє зміни...";

        var verdict = await _aiService.ValidateQuestFairness(currentProposal);

        if (verdict.IsFair)
        {
            IsLoading = false;
            ErrorMessage = "";
            _onQuestCreated(currentProposal);
        }
        else
        {
            ErrorMessage = $"❌ {verdict.Feedback}";
            IsLoading = false;
        }
    }
}