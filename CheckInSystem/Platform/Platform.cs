using CheckinLibrary.Database;
using CheckInSystem.CardReader;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.Views;
using PCSC.Interop;
using System;
using System.Configuration;

namespace CheckInSystem.Platform;
public class Platform : IPlatform
{
    public static IServiceProvider Provider = AppServices.Provider;

    ICardReader _cardReader;
    public ICardReader CardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;

    public event DataLoadedEventHandler? DataLoaded;

    MainWindow _mainWindow;
    public MainWindow MainWindow => _mainWindow;

    IDatabaseHelper _database;
    public IDatabaseHelper Database => _database;

    public Platform()
    {
        _database = CheckinLibrary.Database.Database.DatabaseType();

#if DEBUG || DEBUGINMEMORY || LEGACYDAPPER
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

        _mainWindow = new MainWindow(this)
        {
            DataTemplates = { new ViewLocator() },
            DataContext = _mainWindowViewModel
        };
        _mainWindow.Show();
    }

}
public static class AppServices
{
    public static IServiceProvider Provider { get; set; } = null!;
}