using LifeQuest.Application.Interfaces;

namespace LifeQuest.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;
    private readonly IAiService _aiService;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set { _currentPage = value; OnPropertyChanged(); }
    }

    public MainViewModel(IAiService aiService, IUserRepository userRepository, IQuestRepository questRepository)
    {
        _aiService = aiService;
        _userRepository = userRepository;
        _questRepository = questRepository;
        _currentPage = new LoginViewModel(this);
    }

    public void NavigateToGame(int id, string username)
    {
        CurrentPage = new GameViewModel(id, username, _aiService, _userRepository, this);
    }

    public void NavigateToCreateQuest(GameViewModel gameVm)
    {
        CurrentPage = new CreateQuestViewModel(
            _aiService,
            proposal => {
                gameVm.AddQuestFromAi(proposal);
                NavigateBackToGame(gameVm);
            },
            () => NavigateBackToGame(gameVm)
        );
    }

    public void NavigateToSettings(GameViewModel gameVm)
    {
        CurrentPage = new SettingsViewModel(
            gameVm.UserId,
            gameVm.PlayerName,
            "",
            () => NavigateBackToGame(gameVm)
        );
    }

    public void NavigateToTavern(GameViewModel gameVm)
    {
        CurrentPage = new TavernViewModel(
            _aiService,
            () => NavigateBackToGame(gameVm),
            (npcAdvice) => {
                var createQuestVm = new CreateQuestViewModel(
                    _aiService,
                    proposal => {
                        gameVm.AddQuestFromAi(proposal);
                        NavigateBackToGame(gameVm);
                    },
                    () => NavigateBackToGame(gameVm)
                );
                createQuestVm.UserInput = npcAdvice;
                CurrentPage = createQuestVm;
            }
        );
    }

    private void NavigateBackToGame(GameViewModel gameVm)
    {
        CurrentPage = gameVm;
    }
}