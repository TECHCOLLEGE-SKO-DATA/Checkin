using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace CheckinSystemAvalonia.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected IPlatform _platform;

    public ViewModelBase(IPlatform platform)
    {
        _platform = platform;
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    protected void SetProperty<T>(ref T variable, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(variable, value))
        {
            variable = value;
            OnPropertyChanged(propertyName);
        }
    }
}
