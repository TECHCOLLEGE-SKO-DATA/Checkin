using Avalonia.Controls;
using Avalonia.Media;
using CheckinLib.Platform;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection;
using CheckinLib.Models;
using System.ComponentModel;
using System.Windows.Data;
using CheckinLib.Database;
using CheckinLib.ViewModels.Windows;

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

        ObservableCollection<Group> _groups = new();

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

        public EmployeeOverviewViewModel(IPlatform platform) : base(platform)
        {

            SortEmployees();


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

        private void SortEmployees()
        {
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
