namespace LifeQuest.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _mainNavigator;
    
    private string _username = "";
    private string _password = "";

    public string Username 
    { 
        get => _username; 
        set { _username = value; OnPropertyChanged(); } 
    }

    public string Password 
    { 
        get => _password; 
        set { _password = value; OnPropertyChanged(); } 
    }

    public LoginViewModel(MainViewModel mainNavigator)
    {
        _mainNavigator = mainNavigator;
    }

    public void LoginCommand()
    {
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            // У майбутньому тут буде перевірка пароля в базі даних
            // Поки що просто пускаємо в гру
            _mainNavigator.NavigateToGame(Username);
        }
    }
}