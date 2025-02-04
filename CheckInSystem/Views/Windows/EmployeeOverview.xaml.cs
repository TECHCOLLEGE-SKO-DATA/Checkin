using System;
using System.Windows;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.Windows;
using System.Threading.Tasks;

namespace CheckInSystem.Views
{
    public partial class EmployeeOverview : Window
    {
        private EmployeeOverviewViewModel _vm;
        private BackgroundTimeService _timeService;

        public EmployeeOverview(EmployeeOverviewViewModel vm)
        {
            _vm = vm;
            this.DataContext = _vm;
            InitializeComponent();

            // Start the background time service when the window opens
            StartBackgroundService();
        }

        private void StartBackgroundService()
        {
            _timeService = new BackgroundTimeService();
            _timeService.OnDailyReset += UpdateUIOnReset; // Subscribe to daily reset event
            _timeService.Start();
        }

        private void UpdateUIOnReset()
        {
            Dispatcher.Invoke(() =>
            {
                // Example: Refreshing the view model or adding a message
                // vm.RefreshData();  Add this method in ViewModel to reload data
                //MessageBox.Show("Daily reset has been processed!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void BtnZoomOut(object sender, RoutedEventArgs e)
        {
            _vm.ZoomOut();
            _vm.UpdateConfig();
        }

        private void BtnZoomIn(object sender, RoutedEventArgs e)
        {
            _vm.ZoomIn();
            _vm.UpdateConfig();
        }

        private void BtnFullScreenToggle(object sender, RoutedEventArgs e)
        {
            _vm.ToggleFullscreen();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _timeService?.Stop();  // Stop the background service when the window is closed
        }
    }
}
