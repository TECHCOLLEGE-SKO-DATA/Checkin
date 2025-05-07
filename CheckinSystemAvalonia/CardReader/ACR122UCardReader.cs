using CheckinSystemAvalonia.Platform;
using FrApp42.ACR122U;
using PCSC;
using PCSC.Iso7816;
using System;
namespace CheckinSystemAvalonia.CardReader;

public class ACR122UCardReader : Platform.ICardReader
{
    public event CardScannedEventHandler? CardScanned;

    public event ReaderConnectedEventHandler? ReaderConnected;
    public event ReaderConnectedEventHandler? ReaderDisconnected;
    public event ReaderConnectedEventHandler? CardInserted;
    public event ReaderConnectedEventHandler? CardRemoved;

    [Flags]
    public enum LedStateControl
    {
        BlinkingMaskGreen = 0b1000_0000,
        BlinkingMastRed = 0b0100_0000,
        InitialblinkingStateGreen = 0b0010_0000,
        InitialblinkingStateRed = 0b0001_0000,
        StateMaskGreen = 0b0000_1000,
        StateMaskRed = 0b0000_0100,
        FinalStateGreen = 0b0000_0010,
        FinalStateRed = 0b0000_0001,
    }

    Reader _acr122uReader = new();

    public ACR122UCardReader() 
    {
        Start();
    }
    public void Start() 
    {
        _acr122uReader.Connected += (value) => {
            try
            {
                CardScanned?.Invoke(this, new CardScannedEventArgs() {CardId = value});
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            ReaderConnected?.Invoke(this, new ReaderEventArgs(){ Value = value});
        };
        _acr122uReader.Disconnected += (value) => ReaderDisconnected?.Invoke(this, new ReaderEventArgs(){ Value = value}); //OnReaderDisconnected;
        _acr122uReader.Inserted += (value) => CardInserted?.Invoke(this, new ReaderEventArgs(){ Value = value}); // OnCardInserted;
        _acr122uReader.Removed += () => CardRemoved?.Invoke(this, new ReaderEventArgs());
    }


    bool SignalError()
    {
        var ctx = ContextFactory.Instance.Establish(SCardScope.System);
        var name = _acr122uReader.GetDevice();

        var isoReader = new IsoReader(
            context: ctx,
            readerName: name,
            mode: SCardShareMode.Shared,
            protocol: SCardProtocol.Any,
            releaseContextOnDispose: true);
        var apdu = new CommandApdu(IsoCase.Case3Short, isoReader.ActiveProtocol) {
            CLA = 0xFF,
            Instruction = 0x00,
            P1 = 0x40,
            P2 = (byte) LedStateControl.BlinkingMastRed,
            Data = new byte[]
            {
                0x1, //T1 Duration Initial Blinking State (Unit = 100 ms)
                0x2, //T2 Duration Toggle Blinking State (Unit = 100 ms)
                0x2, //Number of repetition
                0x2, //Link to Buzzer (00h - 03h)
            },
        };

        var response = isoReader.Transmit(apdu);
        
        return response.SW1 == 0x90;
    }

    // void CardScanned(string cardID)
    // {
    //     if (cardID == "")
    //     {
    //         SignalError();
    //         return;
    //     }
        
    //     if (State.UpdateNextEmployee)
    //     {
    //        UpdateNextEmployee(cardID);
    //        return;
    //     }

    //     if (State.UpdateCardId)
    //     {
    //         UpdateCardId(cardID);
    //         return;
    //     }

    //     DatabaseHelper databaseHelper = new();
    //     databaseHelper.CardScanned(cardID);

    //     UpdateEmployeeLocal(cardID);
    // }
}