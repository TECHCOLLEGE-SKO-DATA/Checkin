using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia.Views.UserControls;

public partial class AdminEmployeeView : UserControl
{
    public AdminEmployeeView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}