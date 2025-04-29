using CheckinLib.Platform;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;



namespace CheckinSystemAvalonia.ViewModels;

public class ViewModelBase : ReactiveObject
{

    public ViewModelBase(IPlatform platform)
    {
    }
}
