using CheckinSystemAvalonia.CardReader;
using CheckinSystemAvalonia.ViewModels.Windows;
using System;

namespace CheckinSystemAvalonia.Platform;
public class Platform : IPlatform
{
    ICardReader _cardReader;
    public ICardReader CardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;

    public event DataLoadedEventHandler? DataLoaded;

    public Platform()
    {
#if DEBUG
        _cardReader = new ScriptedCardReader();
#else
        _cardReader = new ACR122UCardReader();
#endif
    }

    public void Start()
    {
        Startup.OpenEmployeeOverview(this);
        _mainWindowViewModel = new(this);
        DataLoaded?.Invoke(this, EventArgs.Empty);
    }

}
