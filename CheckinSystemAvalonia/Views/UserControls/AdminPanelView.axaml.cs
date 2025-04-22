using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckinSystemAvalonia.Views.UserControls;

public partial class AdminPanelView : UserControl
{
    public AdminPanelView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}