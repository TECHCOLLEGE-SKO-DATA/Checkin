using Microsoft.EntityFrameworkCore;

namespace CheckinLibrary.Database.EF
{
    public static class DbFunctions
    {
        [DbFunction("IsEmployeeCheckedIn", "dbo")]
        public static bool IsEmployeeCheckedIn(int employeeId)
            => throw new NotSupportedException();
    }
}
