using System;
using System.Windows.Input;
using LifeQuest.Application.Interfaces;
using LifeQuest.Domain.Entities;

namespace LifeQuest.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _mainNavigator;
    private readonly IUserRepository _userRepository;

    private string _username = "";
    private string _password = "";
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

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public LoginViewModel(MainViewModel mainNavigator, IUserRepository userRepository)
    {
        _mainNavigator = mainNavigator;
        _userRepository = userRepository;

        RegisterCommand = new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "⚠️ Заповніть логін та пароль";
                return;
            }

            var existingUser = _userRepository.GetUserByLogin(Username);
            if (existingUser != null)
            {
                ErrorMessage = "⚠️ Цей логін вже зайнятий";
                return;
            }

            var random = new Random();
            int newId;
            do { newId = random.Next(10000, 99999); }
            while (_userRepository.GetUserById(newId) != null);

            var newUser = new User(newId, Username, "");
            newUser.SetPassword(Password); // ← хешуємо
            _userRepository.SaveUser(newUser);

            ErrorMessage = "";
            _mainNavigator.NavigateToGame(newId, Username);
        });

        LoginCommand = new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "⚠️ Заповніть логін та пароль";
                return;
            }

            var existingUser = _userRepository.GetUserByLogin(Username);
            if (existingUser == null)
            {
                ErrorMessage = "❌ Користувача не знайдено";
                return;
            }

            if (!existingUser.VerifyPassword(Password))
            {
                ErrorMessage = "❌ Невірний пароль";
                return;
            }

            ErrorMessage = "";
            _mainNavigator.NavigateToGame(existingUser.Id, existingUser.Login);
        });
    }
}