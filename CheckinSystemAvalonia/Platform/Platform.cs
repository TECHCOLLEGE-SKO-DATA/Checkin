using CheckInSystemAvalonia.CardReader;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using PCSC.Interop;
using System;

namespace CheckInSystemAvalonia.Platform;
public class Platform : IPlatform
{
    ICardReader _cardReader;
    public ICardReader CardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;

    public event DataLoadedEventHandler? DataLoaded;
    MainWindow _mainWindow;
    public MainWindow MainWindow => _mainWindow;

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

        _mainWindow = new MainWindow
        {
            DataTemplates = { new ViewLocator() },
            DataContext = _mainWindowViewModel
        };
        _mainWindow.Show();
    }

}
