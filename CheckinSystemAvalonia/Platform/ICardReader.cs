using System;

namespace CheckinSystemAvalonia.Platform;
public class ReaderEventArgs : EventArgs
{
    public string Value = "";
}
public class CardScannedEventArgs : EventArgs
{
    public string CardId = "";
}

public delegate void CardScannedEventHandler(object sender, CardScannedEventArgs args);
public delegate void ReaderConnectedEventHandler(object sender, ReaderEventArgs args);

public interface ICardReader
{
    /// <summary>
    /// CardScanned serves as a quarantee that a card was scanned correctly, unlike CardInserted which might not read the entire NFC id
    /// </summary>
    event CardScannedEventHandler? CardScanned;
    event ReaderConnectedEventHandler? ReaderConnected;
    event ReaderConnectedEventHandler? ReaderDisconnected;
    /// <summary>
    /// Fires an event when ever a card was detected, but not necessarily read correctly
    /// </summary>
    event ReaderConnectedEventHandler? CardInserted;
    event ReaderConnectedEventHandler? CardRemoved;
}