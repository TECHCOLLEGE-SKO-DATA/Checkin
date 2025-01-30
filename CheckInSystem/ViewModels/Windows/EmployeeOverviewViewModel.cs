using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Data;
using CheckInSystem.Models;
using System.ComponentModel;
using CheckInSystem.Database;

namespace CheckInSystem.ViewModels.Windows
{
    public class EmployeeOverviewViewModel : ViewModelBase
    {
        private string ConfigFilePath = "";
        private decimal _scaleSize = 1.0M;
        private ResizeMode _resizeMode = ResizeMode.NoResize;
        private WindowStyle _windowStyle = WindowStyle.None;
        WindowState _windowState;

        // Add Groups property
        private ObservableCollection<Group> _groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        public decimal ScaleSize
        {
            get => _scaleSize;
            set => SetProperty(ref _scaleSize, value);
        }

        public ResizeMode ResizeMode
        {
            get => _resizeMode;
            set => SetProperty(ref _resizeMode, value);
        }
        public WindowStyle WindowStyle
        {
            get => _windowStyle;
            set => SetProperty(ref _windowStyle, value);
        }
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        public void ZoomIn()
        {
            ScaleSize += 0.1M;
        }

        public void ZoomOut()
        {
            ScaleSize -= 0.1M;
        }

        public void ToggleFullscreen()
        {
            if (ResizeMode == ResizeMode.NoResize)
            {
                ResizeMode = ResizeMode.CanResizeWithGrip;
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
            else
            {
                ResizeMode = ResizeMode.NoResize;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
        }

        public EmployeeOverviewViewModel()
        {
            string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath += @"\EmployeeOverviewViewModelConfig.txt";
            ConfigFilePath = filePath;
            ReadConfig();

            LoadGroupsAndEmployees(); // Load groups and apply sorting
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
            DatabaseHelper databaseHelper = new ();
            // Fetch employees from the database
            var employees = databaseHelper.GetAllEmployees();

            // Fetch groups and assign employees
            Groups = new ObservableCollection<Group>(Group.GetAllGroups(employees));

            // Apply sorting to each group's Members
            foreach (var group in Groups)
            {
                var view = CollectionViewSource.GetDefaultView(group.Members);
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(nameof(Employee.IsCheckedIn), ListSortDirection.Descending)); // Checked-in first
                view.SortDescriptions.Add(new SortDescription(nameof(Employee.FirstName), ListSortDirection.Ascending));    // Alphabetical
            }
        }
    }
}
