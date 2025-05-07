//using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    ICardReader CardReader { get; }
  //  MainWindowViewModel MainWindowViewModel { get; }
    event DataLoadedEventHandler? DataLoaded;
}