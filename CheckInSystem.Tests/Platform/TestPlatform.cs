using CheckInSystem.CardReader;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.Views;
namespace CheckInSystem.Tests.Platform;

public class TestPlatform : IPlatform
{
    ScriptedCardReader _cardReader = new();
    public ICardReader CardReader => _cardReader;

    public ScriptedCardReader ScriptedCardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;

    public event DataLoadedEventHandler? DataLoaded;


    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;

    MainWindow IPlatform.MainWindow => throw new NotImplementedException();

    public TestPlatform()
    {
        _mainWindowViewModel = new(this);
    }
}