using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystem.Views;

public partial class FakeNFCWindow : Window
{
    public FakeNFCWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}