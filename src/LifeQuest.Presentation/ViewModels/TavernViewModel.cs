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
    private readonly Action _onBack;
    private readonly Action<string> _onCreateQuestFromAdvice;
    
    private string _userInput = "";
    private bool _isLoading = false;

    // Список повідомлень для відображення в UI
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
    public ICommand BackCommand { get; }
    public ICommand CreateQuestCommand { get; } // НОВА КОМАНДА

    public TavernViewModel(IAiService aiService, Action onBack, Action<string> onCreateQuestFromAdvice)
    {
        _aiService = aiService;
        _onBack = onBack;
        _onCreateQuestFromAdvice = onCreateQuestFromAdvice;

        SendCommand = new RelayCommand(async () => await SendMessage());
        BackCommand = new RelayCommand(() => _onBack());

        CreateQuestCommand = new RelayCommand(() => 
        {
            // Беремо останнє повідомлення Елдора
            var lastNpcMsg = ChatHistory.LastOrDefault(m => m.Role.Contains("Елдор"))?.Text;
            if (!string.IsNullOrEmpty(lastNpcMsg))
            {
                _onCreateQuestFromAdvice(lastNpcMsg);
            }
        });
        
        // NPC вітається першим!
        ChatHistory.Add(new ChatMessage { Role = "🧙‍♂️ Гаррі", Text = "Вітаю, мандрівнику! Я Гаррі. Розкажи мені про свою велику ціль, і я допоможу розбити її на дрібні квести." });
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(UserInput)) return;

        var msg = UserInput;
        UserInput = "";

        // Повідомлення гравця
        ChatHistory.Add(new ChatMessage { Role = "😎 Ти", Text = msg });
        IsLoading = true;

        // Запит до Gemini
        var historyList = new System.Collections.Generic.List<ChatMessage>(ChatHistory);
        var response = await _aiService.GetNpcResponse(msg, historyList);

        // Відповідь NPC
        ChatHistory.Add(new ChatMessage { Role = "🧙‍♂️ Елдор", Text = response });
        IsLoading = false;
    }
}