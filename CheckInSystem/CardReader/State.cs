using CheckInSystem.Models;

namespace CheckInSystem.CardReader;

public class State
{ 
    public static bool UpdateNextEmployee { get; set; }
    public static bool UpdateCardId { get; set; }
    public static Employee? EmployeeToUpdate { get; set; }

    public static void SetUpdateCard(Employee updateEmployee)
    {
        EmployeeToUpdate = updateEmployee;
        UpdateCardId = true;
    }
    
    public static void UpdateCard(string CardID)
    {
        if (EmployeeToUpdate == null) return;
        UpdateCardId = false;
        EmployeeToUpdate.CardID = CardID;
        EmployeeToUpdate.UpdateDb();
        EmployeeToUpdate = null;
    }

    public static void ClearUpdateCard()
    {
        UpdateCardId = false;
        EmployeeToUpdate = null;
    }
}