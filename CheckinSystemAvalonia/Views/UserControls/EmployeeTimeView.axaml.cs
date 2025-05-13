using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia.Views.UserControls;

public partial class EmployeeTimeView : UserControl
{
    public EmployeeTimeView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}