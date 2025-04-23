using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckinSystemAvalonia.Views;

public partial class EditEmployeeWindow : Window
{
    public EditEmployeeWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}