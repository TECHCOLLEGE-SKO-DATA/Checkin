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
        //if app.config service name is either of the normal sqlserver type names use the DatabaseHelper else its sqlite
        string serviceName = ConfigurationManager.AppSettings["SqlServiceName"]?.Trim() ?? "";
        
        if (serviceName == "MSSQL$SQLEXPRESS" || serviceName == "MSSQLSERVER")
        {
            _database = new DatabaseHelper();
        }
        else
        {
            _database = new DatabaseSqlLite();
        }

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

        _mainWindow = new MainWindow(this)
        {
            DataTemplates = { new ViewLocator() },
            DataContext = _mainWindowViewModel
        };
        _mainWindow.Show();
    }

}
