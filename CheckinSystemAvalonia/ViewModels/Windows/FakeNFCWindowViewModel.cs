using CheckinLib.Platform;
using CheckinSystemAvalonia.ViewModels.UserControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.Windows
{
    public class FakeNFCWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }
        
        public FakeNFCWindowViewModel(IPlatform platform) : base(platform)
        {
            CurrentViewModel = new FakeNFCViewModel(platform);
        }
    }
}
