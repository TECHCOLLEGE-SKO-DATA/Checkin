using Avalonia.Controls;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;

namespace CheckInSystem.ViewModels.Windows
{
    public class EmployeeOverviewViewModel : ViewModelBase
    {
        private string ConfigFilePath = "";
        private decimal _scaleSize = 1.0M;
        WindowState _windowState;
        
        public string AppVersion
        {
            get
            {
                string? version = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;

                // Remove anything after '+' to keep it clean
                return "v" + (version?.Split('+')[0] ?? "Unknown");
            }
        }
        // Add Groups property
        private ObservableCollection<Group> _groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => this.RaiseAndSetIfChanged(ref _groups, value);
        }

        public decimal ScaleSize
        {
            get => _scaleSize;
            set => this.RaiseAndSetIfChanged(ref _scaleSize, value);
        }

        
        public WindowState WindowState
        {
            get => _windowState;
            set => this.RaiseAndSetIfChanged(ref _windowState, value);
        }

        public void ZoomIn()
        {
            ScaleSize += 0.1M;
            UpdateConfig();
        }

        public void ZoomOut()
        {
            ScaleSize -= 0.1M;
            if (ScaleSize < 0.1M) ScaleSize = 0.1M;
            UpdateConfig();
        }

        public void ToggleFullscreen()
        {
            if (WindowState == WindowState.FullScreen)
            {
                WindowState = WindowState.Normal;
            }
            else 
            {
                WindowState = WindowState.FullScreen;
            }
        }

        //buttons
        public ReactiveCommand<Unit, Unit> Btn_ZoomIn {  get; set; }

        public ReactiveCommand<Unit, Unit> Btn_ZoomOut { get; set; }

        public ReactiveCommand<Unit, Unit> Btn_ToggleFullscreen {  get; set; }

        public EmployeeOverviewViewModel(IPlatform platform) : base(platform)
        {
            
            //button bindings
            Btn_ZoomIn = ReactiveCommand.Create(() => ZoomIn());

            Btn_ZoomOut = ReactiveCommand.Create(() => ZoomOut());

            Btn_ToggleFullscreen = ReactiveCommand.Create(() => ToggleFullscreen());

            //toggles fullscreen to start in fullscreen
            ToggleFullscreen();

            
            string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath += @"\EmployeeOverviewViewModelConfig.txt";
            ConfigFilePath = filePath;
            ReadConfig();



            platform.DataLoaded += (sender, args) =>
            {
                LoadGroupsAndEmployees(); // Load groups and apply sorting
                SortEmployees();
            };

            platform.CardReader.CardRemoved += async (sender, args) =>
            {
                await Task.Delay(10);
                //Sort again
                SortEmployees();

                foreach(var group in Groups)
                {
                    Debug.WriteLine(group.Members);
                }
            };

        }

        // TODO: Consider moving ReadConfig() and UpdateConfig to a config class and use a proper saving format
        private void ReadConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                File.WriteAllText(ConfigFilePath, ScaleSize.ToString());
                return;
            }

            try
            {
                string contents = File.ReadAllText(ConfigFilePath);
                ScaleSize = Convert.ToDecimal(contents);
            }
            catch
            {
                File.Delete(ConfigFilePath);
                UpdateConfig();
            }
        }

        public void UpdateConfig()
        {
            File.WriteAllText(ConfigFilePath, ScaleSize.ToString());
        }

        // New Method: Load groups and apply sorting
        private void LoadGroupsAndEmployees()
        {
            // Fetch employees from the database

            // Fetch groups and assign employees
            //Groups = new ObservableCollection<Group>(Group.GetAllGroups(Employees.ToList()));
            Groups = _platform.MainWindowViewModel.Groups;
        }

        public void SortEmployees()
        {
            ObservableCollection<Group> tempGroups = new();
            foreach (var group in Groups)
            {
                var sortedMembers = group.Members
                    .OrderByDescending(m => m.IsCheckedIn)  // true first
                    .ThenBy(m => m.FirstName)
                    .ToList();

                group.Members.Clear();
                foreach (var member in sortedMembers)
                {
                    group.Members.Add(member);
                }
                tempGroups.Add(group);
            }
            
            Groups = tempGroups;
        }
 
        public List<Employee> UpdateAllEmployees(List<Employee> employees)
        {
            var employeeById = employees.ToDictionary(e => e.ID);

            foreach (var group in Groups)
            {
                foreach (var member in group.Members)
                {
                    if (employeeById.TryGetValue(member.ID, out var updated))
                    {
                        member.IsCheckedIn = updated.IsCheckedIn;
                        member.IsOffSite = updated.IsOffSite;
                    }
                }
            }

            return Groups
                .SelectMany(g => g.Members)
                .ToList();
        }
    }
}
