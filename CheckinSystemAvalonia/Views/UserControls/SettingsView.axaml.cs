using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia.Views.UserControls;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}