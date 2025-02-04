using CheckInSystem.CardReader;
using CheckInSystem.Platform;
namespace CheckInSystem.Tests.Platform;

public class TestPlatform : IPlatform
{
    ScriptedCardReader _cardReader = new();
    public ICardReader CardReader => _cardReader;

    public ScriptedCardReader ScriptedCardReader => _cardReader;
}