using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
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

    public AdminLoginViewModel()
    {
        
        Btn_Login_Click = ReactiveCommand.Create(() => Yes());
    }

    private void Yes()
    {    }

    public string Greeting => "Welcome to Avalonia!";

}
