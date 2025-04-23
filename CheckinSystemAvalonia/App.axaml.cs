using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CheckinSystemAvalonia.ViewModels;
using CheckinSystemAvalonia.Views;
using CheckinSystemAvalonia.Database;

namespace CheckinSystemAvalonia
{
    public partial class App : Application
    {
        DatabaseHelper database;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                var mainWindow = new MainWindow
                {
                    DataTemplates = { new ViewLocator() },
                    DataContext = new ViewModels.Windows.MainWindowViewModel()
                };
                mainWindow.Show();

                var employeeWindow = new EmployeeOverviewWindow
                {
                    DataContext = new ViewModels.Windows.EmployeeOverviewViewModel()
                };
                employeeWindow.Show();
                
                #if DEBUG
                var FakeNFC = new FakeNFCWindow
                {
                    DataTemplates = { new ViewLocator() },
                    DataContext = new ViewModels.Windows.FakeNFCWindowViewModel()
                };
                FakeNFC.Show();
                #endif
            }
            if (Database.Database.EnsureDatabaseAvailable()) 
            { }
            base.OnFrameworkInitializationCompleted();
        }
        public void OpenMessageBox(string title, string errorMessage)
        {
            var messageBox = new MessageBoxWindow
            {
                DataTemplates = { new ViewLocator() },
                DataContext = new ViewModels.Windows.MessageBoxViewModel(title, errorMessage),
            };
            messageBox.Show();
        }
    }
}
