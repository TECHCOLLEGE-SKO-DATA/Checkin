using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia;

public partial class LoadingStartup : Window
{
    public static LoadingStartup? Instance { get; private set; }
    public LoadingStartup()
    {
        InitializeComponent();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}