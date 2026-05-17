using Avalonia.Controls;
using LifeQuest.Presentation.ViewModels;

namespace LifeQuest.Presentation.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void AvatarButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button btn && DataContext is SettingsViewModel vm)
        {
            vm.SelectedAvatar = btn.Tag?.ToString() ?? "⚔️";
        }
    }
}