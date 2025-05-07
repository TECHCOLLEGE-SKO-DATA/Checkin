using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;



namespace CheckinSystemAvalonia.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected IPlatform _platform;

    public ViewModelBase(IPlatform platform)
    {
        _platform = platform;
    }
}
