using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CheckinSystemAvalonia.ViewModels;
using CheckinSystemAvalonia.Views;

namespace CheckinSystemAvalonia
{
    public partial class App : Application
    {
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
                    DataContext = new MainWindowViewModel()
                };
                mainWindow.Show();

                var employeeWindow = new EmployeeOverviewWindow
                {
                    DataContext = new EmployeeOverviewWindowViewModel()
                };
                employeeWindow.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
