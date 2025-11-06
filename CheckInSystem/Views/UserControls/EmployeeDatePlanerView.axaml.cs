using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystem.Views.UserControls;

public partial class EmployeeDatePlanerView : UserControl
{
    public EmployeeDatePlanerView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}