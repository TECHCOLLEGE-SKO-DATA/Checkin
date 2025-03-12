using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;

namespace CheckInSystem.ViewModels.UserControls;

public delegate void LoginSuccessful(object sender, EventArgs args);

public class LoginScreenViewModel : ViewModelBase
{
    public event LoginSuccessful? LoginSuccessful;
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
            LoginSuccessful?.Invoke(this, new());
        }
    }
}