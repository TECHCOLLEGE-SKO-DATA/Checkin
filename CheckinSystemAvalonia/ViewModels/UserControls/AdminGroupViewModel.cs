using Avalonia.Controls;
using CheckinLib.Models;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckinSystemAvalonia.ViewModels.UserControls
{
    public class AdminGroupViewModel : ViewModelBase
    {
        MainWindow main;

        ObservableCollection<Group> _groups = new();
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value, nameof(Groups));
        }

        //Buttons
        public ReactiveCommand<Unit, Unit> Btn_AdminPanel { get; }

        public ReactiveCommand<Unit,Unit> Btn_LoginView { get; }

        public ReactiveCommand<Unit, Unit> Btn_HideAll { get; }

        public ReactiveCommand<Unit,Unit> Btn_ShowAll { get; }

        public ReactiveCommand<Group, Unit> Btn_DeletGroup { get; }

        public ReactiveCommand<Unit,Task> Btn_AddGroup { get; }

        public AdminGroupViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                Groups = platform.MainWindowViewModel.Groups;
            };

            //View/ViewModel Swithcing
            Btn_AdminPanel = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToAdminPanel());
            
            Btn_LoginView = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToLoginView());
            
            //change all groups to hide and show groups for everyone
            Btn_HideAll = ReactiveCommand.Create(() => 
            {
                foreach (var group in Groups) 
                {
                    if (group.Isvisible)
                    {
                        group.Isvisible = false;
                    }
                }
            });

            Btn_ShowAll = ReactiveCommand.Create(() =>
            {
                foreach (var group in Groups)
                {
                    if (!group.Isvisible)
                    {
                        group.Isvisible = true;
                    }
                }
            });

            //Delete Groups by removing from database and the local list
            Btn_DeletGroup = ReactiveCommand.Create<Group>(group =>
            {
                group.RemoveGroupDb();
                Groups.Remove(group);
            });

            //opens window to add group to db
            Btn_AddGroup = ReactiveCommand.Create(async () =>
            { 
                InputDialog inputDialog = new InputDialog("Indtast navn til gruppen");
                var result = await inputDialog.ShowDialog<bool>(_platform.MainWindow);
                if (result == true)
                {
                    if(inputDialog.Answer != "") 
                    {
                        Groups.Add(Group.NewGroup(inputDialog.Answer));
                    }
                }
            });
        }
    }
}
