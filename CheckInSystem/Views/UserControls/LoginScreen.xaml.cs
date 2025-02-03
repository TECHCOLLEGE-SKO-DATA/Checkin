using System.Windows;
using System.Windows.Controls;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;

namespace CheckInSystem.Views.UserControls;

public partial class LoginScreen : UserControl
{
    private LoginScreenViewModel _vm;
    public LoginScreen(LoginScreenViewModel vm)
    {
        _vm = vm;
        DataContext = _vm;
        InitializeComponent();
        KeyDown += _vm.LoginKeyPressed;
    }
    
    private void Login_clicked(object sender, RoutedEventArgs e)
    {
        _vm.AdminLogin();
    }

    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
        _vm.Password = ((PasswordBox)sender).Password; 
    }
}