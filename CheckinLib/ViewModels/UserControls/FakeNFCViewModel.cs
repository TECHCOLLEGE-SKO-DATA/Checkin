using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Documents;
using CheckinLib.Platform;
using CheckinLib.CardReader;
using System.Windows;
using System.Text;
using System;
using CheckinLib.Database;
using CheckinLib.Models;

namespace CheckinLib.ViewModels.UserControls;

public class FakeNFCViewModel : ViewModelBase
{
    DatabaseHelper dbHelper = new();

    private static Random random = new Random();

    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    public string NewCardId { get; set; }

    ScriptedCardReader _cardReader => (ScriptedCardReader) _platform.CardReader;

    public FakeNFCViewModel(IPlatform platform) : base(platform)
    {

        NewCardId = RandomCardGen();

        Employees = new ObservableCollection<Employee>(dbHelper.GetAllEmployees());
    }

    public void ScanNewCard()
    {
        if(NewCardId.Length == 11)
        {
            //Add the Actual method for scaning new car
            _cardReader.TriggerCardInserted(NewCardId);
        }
        else
        {
            
            _cardReader.TriggerCardInserted(RandomCardGen());
        }
        Employees.Clear();
        foreach (var employee in dbHelper.GetAllEmployees())
        {
            Employees.Add(employee);
        }
    }

    public void CheckIn(Employee employee)
    {
        //Add the Actual method for checkin/out 
        //dbHelper.CardScanned(employee.CardID);   
        
        _cardReader.TriggerCardInserted(employee.CardID);
    }

    public ObservableCollection<Employee> GetDataFromDB()
    {
        Employees.Clear();
        foreach (var employee in dbHelper.GetAllEmployees())
        {
            Employees.Add(employee); 
        }
        return Employees;
    }

    public string RandomCardGen()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < 11; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }

}
