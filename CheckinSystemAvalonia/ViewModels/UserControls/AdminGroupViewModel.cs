using Avalonia.Controls;
using CheckinLib.Models;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.UserControls
{
    public class AdminGroupViewModel : ViewModelBase
    {
        ObservableCollection<Group> _groups = new();
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value, nameof(Groups));
        }

        //Buttons
        public ReactiveCommand<Unit, Unit> Btn_AdminPanel { get; }

        public ReactiveCommand<Unit,Unit> Btn_LoginView { get; }

        public AdminGroupViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                Groups = platform.MainWindowViewModel.Groups;
            };

            Btn_AdminPanel = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToAdminPanel());
            Btn_LoginView = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToLoginView());
        }
    }
}
