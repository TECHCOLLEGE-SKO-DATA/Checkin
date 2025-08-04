using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia;

public partial class UpdateAdmin : Window
{
    public string AdminName { get; set; }

    public string UserName { get; set; }

    public UpdateAdmin()
    {
        InitializeComponent();
        
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private  void Btn_SaveAdmin(object sender, RoutedEventArgs e)
    {

        Close();
    }
}