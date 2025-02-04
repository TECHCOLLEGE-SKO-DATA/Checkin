using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.Windows;

namespace CheckInSystem.ViewModels.Windows;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IPlatform platform) : base(platform)
    {
        platform.CardReader.CardScanned += (sender, args) => EmployeeCardScanned(args.CardId);
    }

    public void EmployeeCardScanned(string cardID) 
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

        UpdateEmployeeLocal(cardID);
    }
    
    void UpdateNextEmployee(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        State.UpdateNextEmployee = false;
        Employee? editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (editEmployee == null)
        {
            databaseHelper.CardScanned(cardID);
            editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        }
        if (Views.Dialog.WaitingForCardDialog.Instance != null) 
            Application.Current.Dispatcher.Invoke( () => {
                Views.Dialog.WaitingForCardDialog.Instance.Close();
            });
        
        EditEmployeeWindow.Open(editEmployee);
    }

    void UpdateEmployeeLocal(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        Employee? employee = ViewModelBase.Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (employee != null)
        {
            databaseHelper.CardScanned(cardID); //Update DB
            employee.CardScanned(cardID); //Update UI
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
    
    private void UpdateCardId(string cardID)
    {
        State.UpdateCard(cardID);
    }
}