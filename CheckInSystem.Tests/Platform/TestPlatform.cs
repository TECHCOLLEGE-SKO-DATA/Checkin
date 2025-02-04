using CheckInSystem.CardReader;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows;
namespace CheckInSystem.Tests.Platform;

public class TestPlatform : IPlatform
{
    ScriptedCardReader _cardReader = new();
    public ICardReader CardReader => _cardReader;

    public ScriptedCardReader ScriptedCardReader => _cardReader;

    MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel => _mainWindowViewModel;
    public TestPlatform()
    {
        _mainWindowViewModel = new(this);
    }
}