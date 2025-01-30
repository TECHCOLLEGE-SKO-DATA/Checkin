using CheckInSystem.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CheckInSystem.Database;

namespace CheckInSystem.ViewModels.UserControls;

public class FakeNFCViewModel
{
    DatabaseHelper dbHelper = new();

    public List<Employee> Employees { get; set; }

    public Employee SelectedEmployee { get; set; }

    public bool IsAddButtonDisabled { get; set; }

    public FakeNFCViewModel()
    {
        IsAddButtonDisabled = true;

        Employees = new List<Employee>
        {
            new Employee("Konrad", "Denis", "Jensen", "abc123die24"),
            new Employee("Jhon", "Hoxer", "Test", "abc123die23"),
            new Employee("Konrad", "Carmin", "Johnson", "abc123die22"),
            new Employee("Emil", "Joseph", "Nilsen", "Abv123die12")
        };
    }

    public void CheckIn(Employee employee)
    {
        dbHelper.CardScanned(employee.CardID);   
    }
    public void AddTest()
    {
        if (IsAddButtonDisabled == false)
        {
            foreach (Employee employee in Employees)
            {
                dbHelper.CardScanned(employee.CardID);
                Employee employeeForId = DatabaseHelper.GetFromCardId(employee.CardID);
                dbHelper.UpdateDb(employee.CardID, employee.FirstName, employee.MiddleName, 
                    employee.LastName, false, DateTime.Now, employeeForId.ID);
            }
        }
    }
}
