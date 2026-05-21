using System;
using System.Collections.Specialized;
using Avalonia.Controls;
using LifeQuest.Presentation.ViewModels;

namespace LifeQuest.Presentation.Views;

public partial class TavernView : UserControl
{
    public TavernView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is TavernViewModel vm)
        {
            vm.ChatHistory.CollectionChanged += OnChatChanged;
        }
    }

    private void OnChatChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var scrollViewer = this.FindControl<ScrollViewer>("ChatScrollViewer");
        scrollViewer?.ScrollToEnd();
    }
}