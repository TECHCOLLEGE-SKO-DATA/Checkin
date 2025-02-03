using System.ComponentModel;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.ViewModels;
using CheckInSystem.Views.UserControls;
using CheckInSystem.Views.Windows;

namespace CheckInSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
        ViewModelBase.MainContentControl = MainContent;
        MainContent.Content = new LoginScreen();
    }
    
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
    #region HOMELESS METHODS
    private static void CardScanned(string cardID)
    {
        if (State.UpdateNextEmployee)
        {
           UpdateNextEmployee(cardID);
           return;
        }

        if (State.UpdateCardId)
        {
            UpdateCardId(cardID);
            return;
        }

        DatabaseHelper databaseHelper = new();
        databaseHelper.CardScanned(cardID);

        UpdateEmployeeLocal(cardID);
    }

    private static void UpdateEmployeeLocal(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        Employee? employee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (employee != null)
        {
            employee.CardScanned(cardID);
        }
        else
        {
            var dbEmployee = databaseHelper.GetFromCardId(cardID);
            if (dbEmployee != null)
            {
                Application.Current.Dispatcher.Invoke( () => {
                    ViewModelBase.Employees.Add(dbEmployee);
                });
            }
        }
    }

    private static void UpdateNextEmployee(string cardID)
    {
        State.UpdateNextEmployee = false;
        Employee? editEmployee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (editEmployee == null)
        {
            CardScanned(cardID);
            editEmployee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        }
        if (Views.Dialog.WaitingForCardDialog.Instance != null) 
            Application.Current.Dispatcher.Invoke( () => {
                Views.Dialog.WaitingForCardDialog.Instance.Close();
            });
        
        EditEmployeeWindow.Open(editEmployee);
    }

    private static void UpdateCardId(string cardID)
    {
        State.UpdateCard(cardID);
    }
#endregion
}