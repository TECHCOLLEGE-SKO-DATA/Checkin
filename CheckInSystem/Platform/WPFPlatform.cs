using CheckInSystem.CardReader;

namespace CheckInSystem.Platform;

public class WPFPlatform : IPlatform
{
    ICardReader _cardReader;
    public ICardReader CardReader => _cardReader;

    public WPFPlatform()
    {
#if DEBUG
        _cardReader = new ScriptedCardReader();
#else
        _cardReader = new ACR122UCardReader();
#endif
    }
}