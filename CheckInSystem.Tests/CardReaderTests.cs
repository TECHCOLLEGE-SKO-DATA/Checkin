using CheckInSystem.Tests.Platform;
using PCSC.Exceptions;
using Xunit;
namespace CheckInSystem.Tests;
public class CardReaderTests
{
    
    const string CARD1 = "df 2e 22 1a";
    [Fact]
    public void CanTriggerCardInserted()
    {
        TestPlatform _platform = new();
        int triggerCounter = 0;

        //Test that event is triggered
        bool success = false;
        _platform.CardReader.CardInserted += (sender, args) => {
            success = true;
            triggerCounter++;
        };
        _platform.ScriptedCardReader.TriggerCardInserted(CARD1);
        Assert.True(success);

        //Test that the card id is received correctly
        _platform = new();
        string cardId = "";
        _platform.ScriptedCardReader.CardInserted += (sender, args) => {
            cardId = args.Value;
            triggerCounter++;
        };

        _platform.ScriptedCardReader.TriggerCardInserted(CARD1);
        Assert.Equal(CARD1, cardId);

        //Test that it is not triggered more than it should
        Assert.Equal(2, triggerCounter);
    }

    [Fact]
    public void IsCardScannedWhenInserted()
    {
        TestPlatform _platform = new();
        bool success = false;
        //Test that CardScanned is triggered if CardId is present
        _platform.CardReader.CardScanned += (sender, args) => {
            success = true;
        };
        _platform.ScriptedCardReader.TriggerCardInserted(CARD1);
        Assert.True(success);

        //Test that that CardScanned does not trigger if Card ID is not read correctly
        _platform = new();
        success = true;

        _platform.CardReader.CardScanned += (sender, args) => {
            success = false;
        };

        _platform.ScriptedCardReader.TriggerCardInserted(string.Empty);
        Assert.True(success);
        

    }

    [Fact]
    public void CanTriggerReaderConnected()
    {
        TestPlatform _platform = new();
        int triggerCounter = 0;
        bool success = false;
        _platform.CardReader.ReaderConnected += (sender, args) => {
            success = true;
            triggerCounter++;
        };
        _platform.ScriptedCardReader.TriggerReaderConnected("");
        Assert.True(success);

        //Test that it is not triggered more than it should
        Assert.Equal(1, triggerCounter);
    }

    //Todo: Test CardRemoved, ReaderDisconnected
}