using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckInSystemAvalonia.Platform;
using System.Diagnostics;

namespace CheckInSystemAvalonia;
public partial class UpdateAdmin : Window
{
    public UpdateAdmin(IPlatform platform)
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Btn_SaveAdmin(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"{Username} : {Password}");
        Close(true);
    }

    public string Username => txtUserName?.Text;
    public string Password => txtPassword?.Text;
}
