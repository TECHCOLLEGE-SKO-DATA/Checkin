using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CheckinSystemAvalonia.Views.UserControls;

public partial class AdminLoginView : UserControl
{
    public AdminLoginView()
    {
        InitializeComponent();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
