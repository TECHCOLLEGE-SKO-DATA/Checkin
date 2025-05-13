

using System.Diagnostics;
using CheckInSystemAvalonia.Platform;

namespace CheckInSystemAvalonia.CardReader;

public class ScriptedCardReader : ICardReader
{
    public event CardScannedEventHandler? CardScanned;
    public event ReaderConnectedEventHandler? ReaderConnected;
    public event ReaderConnectedEventHandler? ReaderDisconnected;
    public event ReaderConnectedEventHandler? CardInserted;
    public event ReaderConnectedEventHandler? CardRemoved;
    public ScriptedCardReader() {
        Debug.WriteLine("Created instance of scripted card reader");
    }

    public void TriggerReaderConnected(string value) 
    {
        ReaderConnected?.Invoke(this, new () { Value = value });
    }
    public void TriggerReaderDisconnected(string value) 
    {
        ReaderDisconnected?.Invoke(this, new () { Value = value });
    }
    public void TriggerCardInserted(string value) 
    {
        CardInserted?.Invoke(this, new () { Value = value });
        if (!string.IsNullOrWhiteSpace(value)) {
            CardScanned?.Invoke(this, new () { CardId = value });
        }
    }
    public void TriggerCardRemoved(string value) 
    {
        CardRemoved?.Invoke(this, new () { Value = value });
    }
}