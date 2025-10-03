using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystem.Views.UserControls;

public partial class AdminEmployeeOverviewView : UserControl
{
    public AdminEmployeeOverviewView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}