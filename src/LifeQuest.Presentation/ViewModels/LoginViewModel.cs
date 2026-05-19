using System.Windows.Input;

namespace LifeQuest.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _mainNavigator;

    private string _username = "";
    private string _password = "";
    private int _id = 0;
    private string _errorMessage = "";

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

    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(MainViewModel mainNavigator)
    {
        _mainNavigator = mainNavigator;

        LoginCommand = new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "⚠️ Заповніть логін та пароль";
                return;
            }
            ErrorMessage = "";
            _mainNavigator.NavigateToGame(Id, Username);
        });
    }
}