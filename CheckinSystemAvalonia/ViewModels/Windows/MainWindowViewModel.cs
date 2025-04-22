using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.Windows;
public class MainWindowViewModel : ViewModelBase
{
   private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    
    public MainWindowViewModel()
    {
        CurrentViewModel = new AdminLoginViewModel(this);
    }
}

