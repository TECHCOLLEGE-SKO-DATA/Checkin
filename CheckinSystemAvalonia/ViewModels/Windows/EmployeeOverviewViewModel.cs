using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection;

namespace CheckinSystemAvalonia.ViewModels.Windows
{
    public class EmployeeOverviewViewModel : ViewModelBase
    {
        private decimal _scaleSize = 1.0M;
        private WindowState _windowState = WindowState.Normal;
        private bool _canResize = true;

        public ReactiveCommand<Unit, Unit> ZoomIn { get; private set; }

        public ReactiveCommand<Unit, Unit> ZoomOut { get; private set; }

        public ReactiveCommand<Unit, Unit> ToggleFullscreen { get; private set; }

        private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => this.RaiseAndSetIfChanged(ref _groups, value);
        }

        public string AppVersion
        {
            get
            {
                string version = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;

                return "v" + (version?.Split('+')[0] ?? "Unknown");
            }
        }

        public EmployeeOverviewViewModel(Platform.IPlatform platform)
        {
            LoadDummyData();
            ZoomIn = ReactiveCommand.Create(ZoomInExecuted);
            ZoomOut = ReactiveCommand.Create(ZoomOutExecuted);
            ToggleFullscreen = ReactiveCommand.Create(ToggleFullscreenExecuted);
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

        public bool CanResize
        {
            get => _canResize;
            set => this.RaiseAndSetIfChanged(ref _canResize, value);
        }

        private void ZoomInExecuted()
        {
            ScaleSize += 0.1M;
        }

        private void ZoomOutExecuted()
        {
            ScaleSize -= 0.1M;
            if (ScaleSize < 0.1M) ScaleSize = 0.1M;
        }

        private void ToggleFullscreenExecuted()
        {
            if (_windowState == WindowState.Normal)
            {

                WindowState = WindowState.FullScreen;
                
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void LoadDummyData()
        {
            var employees1 = new List<Employee>
            {
                new Employee { ID = 1, FirstName = "John", MiddleName = "A", LastName = "Doe", IsCheckedIn = true, IsOffSite = false },
                new Employee { ID = 2, FirstName = "Jane", MiddleName = "", LastName = "Smith", IsCheckedIn = false, IsOffSite = false }
            };

            var employees2 = new List<Employee>
            {
                new Employee { ID = 3, FirstName = "Alice", MiddleName = "B", LastName = "Brown", IsCheckedIn = true, IsOffSite = true },
                new Employee { ID = 4, FirstName = "Bob", MiddleName = "C", LastName = "Taylor", IsCheckedIn = false, IsOffSite = false }
            };

            Groups = new ObservableCollection<Group>
            {
                new Group { ID = 101, Name = "Group A", IsVisible = true, Members = new ObservableCollection<Employee>(employees1) },
                new Group { ID = 102, Name = "Group B", IsVisible = true, Members = new ObservableCollection<Employee>(employees2) }
            };
        }

        public class Group : ReactiveObject
        {
            public int ID { get; set; }

            private string _name;
            public string Name
            {
                get => _name;
                set => this.RaiseAndSetIfChanged(ref _name, value);
            }

            private bool _isVisible;
            public bool IsVisible
            {
                get => _isVisible;
                set => this.RaiseAndSetIfChanged(ref _isVisible, value);
            }

            private ObservableCollection<Employee> _members = new();
            public ObservableCollection<Employee> Members
            {
                get => _members;
                set => this.RaiseAndSetIfChanged(ref _members, value);
            }
        }

        public class Employee : ReactiveObject
        {
            public int ID { get; set; }

            private string _firstName;
            public string FirstName
            {
                get => _firstName;
                set => this.RaiseAndSetIfChanged(ref _firstName, value);
            }

            private string _middleName;
            public string MiddleName
            {
                get => _middleName;
                set => this.RaiseAndSetIfChanged(ref _middleName, value);
            }

            public string MiddleNameShort => string.IsNullOrWhiteSpace(MiddleName) ? "" : MiddleName.Substring(0, 1) + ".";

            private string _lastName;
            public string LastName
            {
                get => _lastName;
                set => this.RaiseAndSetIfChanged(ref _lastName, value);
            }

            private bool _isCheckedIn;
            public bool IsCheckedIn
            {
                get => _isCheckedIn;
                set => this.RaiseAndSetIfChanged(ref _isCheckedIn, value);
            }

            private bool _isOffSite;
            public bool IsOffSite
            {
                get => _isOffSite;
                set => this.RaiseAndSetIfChanged(ref _isOffSite, value);
            }
        }
    }
}
