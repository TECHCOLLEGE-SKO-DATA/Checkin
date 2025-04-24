using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;



namespace CheckinSystemAvalonia.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private IPlatform platform;

    public ViewModelBase(IPlatform platform)
    {
        this.platform = platform;
    }
}
