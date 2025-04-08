using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckinSystemAvalonia.Views;

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