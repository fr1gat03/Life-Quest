namespace LifeQuest.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set { _currentPage = value; OnPropertyChanged(); }
    }

    public MainViewModel()
    {
        _currentPage = new LoginViewModel(this);
    }

    // Метод для переходу до гри
    public void NavigateToGame(string username)
    {
        CurrentPage = new GameViewModel(username);
    }
}