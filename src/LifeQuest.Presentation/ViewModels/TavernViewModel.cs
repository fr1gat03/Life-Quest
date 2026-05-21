using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LifeQuest.Application.Interfaces;

namespace LifeQuest.Presentation.ViewModels;

public class TavernViewModel : ViewModelBase
{
    private readonly IAiService _aiService;
    private Action _onBack;
    private readonly Action<string> _onCreateQuestFromAdvice;
    public bool HasNpcMessage => ChatHistory.Any(m => m.Role.Contains("Елдор") && ChatHistory.IndexOf(m) > 0);

    private string _userInput = "";
    private bool _isLoading = false;

    public ObservableCollection<ChatMessage> ChatHistory { get; } = new();

    public string UserInput
    {
        get => _userInput;
        set { _userInput = value; OnPropertyChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ICommand SendCommand { get; }
    public ICommand BackCommand { get; private set; }
    public ICommand CreateQuestFromLastCommand { get; }

    public TavernViewModel(IAiService aiService, Action onBack, Action<string> onCreateQuestFromAdvice)
    {
        _aiService = aiService;
        _onBack = onBack;
        _onCreateQuestFromAdvice = onCreateQuestFromAdvice;

        BackCommand = new RelayCommand(() => _onBack());

        SendCommand = new RelayCommand(async () => await SendMessage());

        CreateQuestFromLastCommand = new RelayCommand(() =>
        {
            var lastNpcMsg = ChatHistory
                .LastOrDefault(m => m.Role.Contains("Елдор"))?.Text;

            if (!string.IsNullOrEmpty(lastNpcMsg))
                _onCreateQuestFromAdvice(lastNpcMsg);
        });

        if (ChatHistory.Count == 0)
        {
            ChatHistory.Add(new ChatMessage
            {
                Role = "🧙‍♂️ Елдор", 
                Text = "Вітаю, мандрівнику! Я Елдор. Розкажи мені про свою велику ціль, і я допоможу розбити її на дрібні квести."
            });
        }
    }

    public void UpdateBackCallback(Action newOnBack)
    {
        _onBack = newOnBack;
        BackCommand = new RelayCommand(() => _onBack());
        OnPropertyChanged(nameof(BackCommand));
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(UserInput)) return;

        var msg = UserInput;
        UserInput = "";

        ChatHistory.Add(new ChatMessage { Role = "😎 Ти", Text = msg });
        IsLoading = true;

        var historyList = new System.Collections.Generic.List<ChatMessage>(ChatHistory);
        var response = await _aiService.GetNpcResponse(msg, historyList);

        ChatHistory.Add(new ChatMessage { Role = "🧙‍♂️ Елдор", Text = response });
        IsLoading = false;
        
        ChatHistory.CollectionChanged += (_, _) => OnPropertyChanged(nameof(HasNpcMessage));
    }
}