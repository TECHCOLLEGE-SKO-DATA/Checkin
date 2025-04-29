using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CheckinLib.Database;
using CheckinLib.Models;
using CheckinLib.Platform;

namespace CheckinLib.ViewModels.UserControls;

public delegate void LoginSuccessful(object sender, EventArgs args);

public class LoginScreenViewModel : ViewModelBase
{
    public event LoginSuccessful? LoginSuccessful;

    public LoginScreenViewModel(IPlatform platform) : base(platform)
    {
        
    }

    public bool AdminLogin(string Username, string Password)
    {
        DatabaseHelper databaseHelper = new ();
        AdminUser? adminUser = databaseHelper.Login(Username, Password);
        if (adminUser == null)
        {
            MessageBox.Show("Forkert brugernavn eller kodeord, prøv igen.", "Login fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else
        {
            LoginSuccessful?.Invoke(this, new());
            return true;
        }
    }
}