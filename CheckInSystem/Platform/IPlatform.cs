namespace CheckInSystem.Platform;

public interface IPlatform
{
    ICardReader CardReader { get; }
}