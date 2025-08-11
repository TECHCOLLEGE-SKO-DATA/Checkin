using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystem.Views.UserControls;

public partial class AdminsView : UserControl
{
    public AdminsView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}