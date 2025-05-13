using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using System;

namespace CheckInSystemAvalonia.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    ICardReader CardReader { get; }
    MainWindowViewModel MainWindowViewModel { get; }
    MainWindow MainWindow { get; }

    event DataLoadedEventHandler? DataLoaded;
}