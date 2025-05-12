using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views;
using System;

namespace CheckinSystemAvalonia.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    ICardReader CardReader { get; }
    MainWindowViewModel MainWindowViewModel { get; }
    MainWindow MainWindow { get; }

    event DataLoadedEventHandler? DataLoaded;
}