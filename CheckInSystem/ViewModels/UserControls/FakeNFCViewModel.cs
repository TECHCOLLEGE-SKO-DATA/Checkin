using CheckInSystem.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CheckInSystem.Database;
using System.Windows.Documents;
using CheckInSystem.Platform;
using CheckInSystem.CardReader;
using System.Windows;
using System.Text;
using System;

namespace CheckInSystem.ViewModels.UserControls;

public class FakeNFCViewModel : ViewModelBase
{
    DatabaseHelper dbHelper = new();

    private static Random random = new Random();

    public List<Employee> TestData { get; set; }

    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    public bool IsAddButtonDisabled { get; set; }

    public string NewCardId { get; set; }

    ScriptedCardReader _cardReader => (ScriptedCardReader) _platform.CardReader;

    public FakeNFCViewModel(IPlatform platform) : base(platform)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < 11; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        NewCardId = result.ToString();

        IsAddButtonDisabled = true;

        Employees = new ObservableCollection<Employee>(dbHelper.GetAllEmployees());

        TestData = new List<Employee>
        {
            new Employee("Konrad", "Denis", "Jensen", "abc123die20"),
            new Employee("Jhon", "Hoxer", "Test", "abc123die23"),
            new Employee("Konrad", "Carmin", "Johnson", "abc123die22"),
            new Employee("Emil", "Joseph", "Nilsen", "Abv123die21")
        };
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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < 11; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            _cardReader.TriggerCardInserted(result.ToString());
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
    public void AddTest()
    {
        if (IsAddButtonDisabled == false)
        {
            foreach (Employee employee in TestData)
            {
                dbHelper.CardScanned(employee.CardID);
                Employee employeeForId = dbHelper.GetFromCardId(employee.CardID);
                dbHelper.UpdateDb(employee.CardID, employee.FirstName, employee.MiddleName, 
                    employee.LastName, false, DateTime.Now, employeeForId.ID);
            }
        }
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



}
