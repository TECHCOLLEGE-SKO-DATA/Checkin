using CheckinLib.CardReader;
using CheckinLib.ViewModels.Windows;

namespace CheckinLib.Platform;
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
        _mainWindowViewModel = new(this);
        _mainWindowViewModel.LoadDataFromDatabase();

        DataLoaded?.Invoke(this, EventArgs.Empty);
    }
}
