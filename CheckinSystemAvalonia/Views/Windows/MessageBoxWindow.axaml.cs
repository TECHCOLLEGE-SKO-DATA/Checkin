using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace CheckInSystemAvalonia.Views;

public partial class MessageBoxWindow : Window
{
    private bool _closedWithResult = false;

    public MessageBoxWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
