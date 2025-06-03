using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.UserControls;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views.UserControls;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace CheckInSystemAvalonia.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected IPlatform _platform;

    public ViewModelBase(IPlatform platform)
    {
        _platform = platform;
    }

    protected void SetProperty<T>(ref T variable, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(variable, value))
        {
            variable = value;
            this.RaisePropertyChanged(propertyName);
        }
    }
}
