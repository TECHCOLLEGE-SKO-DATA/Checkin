using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace CheckinSystemAvalonia.Views;

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

    private void Btn_Ok(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(); 
    }
}
