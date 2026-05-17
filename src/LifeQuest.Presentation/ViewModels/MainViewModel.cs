using System;
using System.Threading.Tasks;
using LifeQuest.Application.Interfaces;
using LifeQuest.Infrastructure.Services;
using LifeQuest.Application.Handlers.QuestExecution;
using LifeQuest.Domain.Entities;
using ReactiveUI;

namespace LifeQuest.Presentation.ViewModels;

public partial class MainViewModel : ReactiveObject
{
    private readonly GeminiAiService _aiService;
    private readonly IUserRepository _userRepository;
    private readonly IQuestRepository _questRepository;
    private readonly BaseQuestHandler _questExecutionChain;

    private ReactiveObject? _currentPage;
    public ReactiveObject? CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    private User? _currentUser;
    public User? CurrentUser
    {
        get => _currentUser;
        set => this.RaiseAndSetIfChanged(ref _currentUser, value);
    }

    public MainViewModel(
        GeminiAiService aiService,
        IUserRepository userRepository,
        IQuestRepository questRepository)
    {
        _aiService = aiService;
        _userRepository = userRepository;
        _questRepository = questRepository;

        var validationHandler = new ValidationHandler();
        var experienceHandler = new ExperienceHandler();
        var persistenceHandler = new PersistenceHandler(_userRepository, _questRepository);

        validationHandler.SetNext(experienceHandler);
        experienceHandler.SetNext(persistenceHandler);

        _questExecutionChain = validationHandler;

        LoadUserData();
    }

    public void NavigateToGame(int userId, string password) => NavigateToTavern(null);

    // Тепер методи приймають 1 аргумент (object?), щоб збігатися з викликами в GameViewModel
    public void NavigateToCreateQuest(object? parameter) => Console.WriteLine("Навігація: Створення квесту");
    public void NavigateToTavern(object? parameter) => Console.WriteLine("Навігація: Таверна");
    public void NavigateToSettings(object? parameter) => Console.WriteLine("Навігація: Налаштування");

    private void LoadUserData()
    {
        var user = _userRepository.GetUserById(1);
        if (user == null)
        {
            user = new User(1, "Герой", "default_pass");
            _userRepository.SaveUser(user);
        }
        CurrentUser = user;
    }

    public async Task ExecuteQuest(QuestExecutionContext context)
    {
        if (_questExecutionChain != null && CurrentUser != null)
        {
            await _questExecutionChain.Handle(context);
            CurrentUser = _userRepository.GetUserById(CurrentUser.Id);
        }
    }
}