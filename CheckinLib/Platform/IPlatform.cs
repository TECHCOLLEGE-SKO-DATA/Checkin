using CheckinLib.ViewModels.Windows;

namespace CheckinLib.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    ICardReader CardReader { get; }
    MainWindowViewModel MainWindowViewModel { get; }
    event DataLoadedEventHandler? DataLoaded;
}