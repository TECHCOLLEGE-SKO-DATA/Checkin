using Avalonia;
using Avalonia.Controls;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using DynamicData;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace CheckinSystemAvalonia.ViewModels.UserControls;

public class AdminLoginViewModel : ViewModelBase
{
    private string _userName;
    public string Username
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    private string _passWord;
    public string PassWord
    {
        get => _passWord;
        set => this.RaiseAndSetIfChanged(ref _passWord, value);
    }

    //buttons
    public ReactiveCommand<Unit, Unit> Btn_Login_Click { get; }

    private MainWindowViewModel _mainWindowViewModel;

    public AdminLoginViewModel(IPlatform platform, MainWindowViewModel mainWindowViewModel) : base(platform)
    {
        _mainWindowViewModel = mainWindowViewModel;

        Btn_Login_Click = ReactiveCommand.Create(() => Login(Username,PassWord));
    }

    private void Login(string userName, string password)
    {
        if(userName == "sko" && password == "test123")
        {
            _mainWindowViewModel.CurrentViewModel = new AdminPanelViewModel(_mainWindowViewModel);
        }
    }

}
