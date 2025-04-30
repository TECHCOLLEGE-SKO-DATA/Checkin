using CheckinSystemAvalonia.ViewModels.Windows;
using System;

namespace CheckinSystemAvalonia.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    ICardReader CardReader { get; }
    MainWindowViewModel MainWindowViewModel { get; }
    event DataLoadedEventHandler? DataLoaded;
}