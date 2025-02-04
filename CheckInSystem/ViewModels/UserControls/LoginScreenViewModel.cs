using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class LoginScreenViewModel : ViewModelBase
{
    private string _username = "";

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password = "";

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public LoginScreenViewModel(IPlatform platform) : base(platform)
    {
        
    }

    public void AdminLogin()
    {
        DatabaseHelper databaseHelper = new ();
        AdminUser? adminUser = databaseHelper.Login(Username, Password);
        if (adminUser == null)
        {
            MessageBox.Show("Forkert brugernavn eller kodeord, prøv igen.", "Login fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            //Move to Adminpanel
            MainContentControl.Content = new AdminPanel(new AdminPanelViewModel(_platform,new()));
        }
    }
    
    public void LoginKeyPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AdminLogin();
        }
    }
}