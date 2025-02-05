using CheckInSystem.CardReader;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Platform;


public class WPFPlatform : IPlatform
{
    ICardReader _cardReader;
    public ICardReader CardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;
    
    public event DataLoadedEventHandler? DataLoaded;

    public WPFPlatform()
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
        _mainWindowViewModel.LoadDataFromDatabase();
        DataLoaded?.Invoke(this, EventArgs.Empty);
    }

}