using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace CheckInSystemAvalonia;

public partial class UpdateAdmin : Window
{

    public UpdateAdmin()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private  void Btn_SaveAdmin(object sender, RoutedEventArgs e)
    {
        Close(true);
    }

    public string Password
    {
        get { return txtPassword.Text; }
    }

    public string Username
    {
        get { return txtUserName.Text; }
    }
}