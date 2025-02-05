using System.ComponentModel;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.UserControls;
using CheckInSystem.Views;
using CheckInSystem.Views.Windows;
using WpfScreenHelper;
using CheckInSystem.Views.Dialog;
namespace CheckInSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly WPFPlatform platform = new();
    MainWindowViewModel _vm { get => (MainWindowViewModel) DataContext; set => DataContext = value; }
    public MainWindowViewModel MainWindowViewModel { get => _vm; set => _vm = value; }
    public MainWindow()
    {
        platform.Start();
        InitializeComponent();
        _vm = platform.MainWindowViewModel;
        
        Closing += OnWindowClosing;
        LoadingStartup.Instance?.Close();
        //ViewModelBase.MainContentControl = MainContent;
        //MainContent.Content = new LoginScreen(new LoginScreenViewModel(platform));

#if DEBUG
        OpenFakeNFCWindow();
#endif
    }
    
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private static void OpenFakeNFCWindow()
    {

        var screens = Screen.AllScreens.GetEnumerator();
        screens.MoveNext();
        Screen? screen = screens.Current;

        FakeNFCWindow fakeNFCWindow = new FakeNFCWindow();
        fakeNFCWindow.DataContext = new FakeNFCWindowViewModel(platform);

        if (screen != null)
        {
            fakeNFCWindow.Top = screen.Bounds.Top;
            fakeNFCWindow.Left = screen.Bounds.Left;
            fakeNFCWindow.Height = 450;
            fakeNFCWindow.Width = 800;
        }

        fakeNFCWindow.Show();
    }

    #region HOMELESS METHODS
    //private static void CardScanned(string cardID)
    //{
    //    if (State.UpdateNextEmployee)
    //    {
    //       UpdateNextEmployee(cardID);
    //       return;
    //    }

    //    if (State.UpdateCardId)
    //    {
    //        UpdateCardId(cardID);
    //        return;
    //    }

    //    DatabaseHelper databaseHelper = new();
    //    databaseHelper.CardScanned(cardID);

    //    UpdateEmployeeLocal(cardID);
    //}

    //private static void UpdateEmployeeLocal(string cardID)
    //{
    //    DatabaseHelper databaseHelper = new();
    //    Employee? employee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
    //    if (employee != null)
    //    {
    //        employee.CardScanned(cardID);
    //    }
    //    else
    //    {
    //        var dbEmployee = databaseHelper.GetFromCardId(cardID);
    //        if (dbEmployee != null)
    //        {
    //            Application.Current.Dispatcher.Invoke( () => {
    //                ViewModelBase.Employees.Add(dbEmployee);
    //            });
    //        }
    //    }
    //}

    //private static void UpdateNextEmployee(string cardID)
    //{
    //    State.UpdateNextEmployee = false;
    //    Employee? editEmployee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
    //    if (editEmployee == null)
    //    {
    //        CardScanned(cardID);
    //        editEmployee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
    //    }
    //    if (Views.Dialog.WaitingForCardDialog.Instance != null) 
    //        Application.Current.Dispatcher.Invoke( () => {
    //            Views.Dialog.WaitingForCardDialog.Instance.Close();
    //        });
        
    //    EditEmployeeWindow.Open(editEmployee);
    //}

    //private static void UpdateCardId(string cardID)
    //{
    //    State.UpdateCard(cardID);
    //}
#endregion
}