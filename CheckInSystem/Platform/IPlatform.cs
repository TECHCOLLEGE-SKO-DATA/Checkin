using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.Views;
using System;

namespace CheckInSystem.Platform;

public delegate void DataLoadedEventHandler(object sender, EventArgs e);

public interface IPlatform
{
    IDatabaseHelper Database { get; }
    ICardReader CardReader { get; }
    MainWindowViewModel MainWindowViewModel { get; }
    MainWindow MainWindow { get; }

    event DataLoadedEventHandler? DataLoaded;
}