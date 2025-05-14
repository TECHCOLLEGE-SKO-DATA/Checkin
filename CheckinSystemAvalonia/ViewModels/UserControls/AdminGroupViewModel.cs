using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckInSystemAvalonia.ViewModels.UserControls
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

        public ReactiveCommand<Unit, Unit> Btn_HideAll { get; }

        public ReactiveCommand<Unit,Unit> Btn_ShowAll { get; }

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
                        group.Updatevisibility(false);
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
                        group.Updatevisibility(true);
                    }
                }
            });

            //opens window to add group to db
            Btn_AddGroup = ReactiveCommand.Create(async () =>
            { 
                InputDialog inputDialog = new InputDialog("Indtast navn til gruppen");

                //opens and waits for answer from the window before continueing
                var result = await inputDialog.ShowDialog<bool>(_platform.MainWindow);

                if (result == true)
                {
                    if(inputDialog.Answer != "") 
                    {
                        //runs both newgroup to add to db but also adds to local collection groups 
                        Groups.Add(Group.NewGroup(inputDialog.Answer));
                    }
                }
            });
        }

        public void DeleteGroup(Group group)
        {
            group.RemoveGroupDb();
            Groups.Remove(group);
        }
        public async Task EditGroupNameAsync(Group group, InputDialog input)
        {
            var result = await input.ShowDialog<bool>(_platform.MainWindow);

            if (result == true)
            {
                if (input.Answer != "")
                {
                    group.UpdateName(input.Answer, group.ID);
                }
            }
        }
    }
}
