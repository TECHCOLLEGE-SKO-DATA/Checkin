using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CheckinLibrary.Models;
using System.Reactive;
using System.ComponentModel;
using System.Windows;
using CheckinLibrary.Database;
using CheckInSystemAvalonia.Platform;
using Avalonia.Controls;

namespace CheckInSystemAvalonia.ViewModels.Windows
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
        }

        public void ZoomOut()
        {
            ScaleSize -= 0.1M;
            if (ScaleSize < 0.1M) ScaleSize = 0.1M;
        }

        public void ToggleFullscreen()
        {
            
        }

        public EmployeeOverviewViewModel(IPlatform platform) : base(platform)
        {
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

            platform.CardReader.CardScanned += async (sender, args) =>
            {
                //to ensure correct sorting 10 millisecond delay
                await Task.Delay(10);

                //Sort again
                SortEmployees();
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
            DatabaseHelper databaseHelper = new();
            // Fetch employees from the database

            // Fetch groups and assign employees
            //Groups = new ObservableCollection<Group>(Group.GetAllGroups(Employees.ToList()));
            Groups = _platform.MainWindowViewModel.Groups;

        }

        private void SortEmployees()
        {
            foreach (var group in Groups)
            {
                // Sort using LINQ and recreate the collection
                var sorted = group.Members
                    .OrderByDescending(e => e.IsCheckedIn)
                    .ThenBy(e => e.FirstName)
                    .ToList();

                group.Members.Clear();
                foreach (var employee in sorted)
                {
                    group.Members.Add(employee);
                }
            }
        }

    }
}
