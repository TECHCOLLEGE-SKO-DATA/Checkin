using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckinSystemAvalonia;

public partial class EmployeeOverviewWindow : Window
{
    public EmployeeOverviewWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}