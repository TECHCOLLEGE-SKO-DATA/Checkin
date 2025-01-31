using CheckInSystem.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CheckInSystem.Database;
using System.Windows.Documents;

namespace CheckInSystem.ViewModels.UserControls;

public class FakeNFCViewModel : ViewmodelBase
{
    DatabaseHelper dbHelper = new();

    public List<Employee> TestData { get; set; }

    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    public Employee SelectedEmployee { get; set; }

    public bool IsAddButtonDisabled { get; set; }
    public string NewCardId { get; set; }

    public FakeNFCViewModel()
    {
        NewCardId = "";

        IsAddButtonDisabled = true;

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
            dbHelper.CardScanned(NewCardId);
        }
    }

    public void CheckIn(Employee employee)
    {
        //Add the Actual method for checkin/out 
        dbHelper.CardScanned(employee.CardID);   
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
