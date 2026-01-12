using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("onSiteTime")]
public class OnSiteTimeDTO
{
    [Key]
    public int Id { get; set; }

    public int EmployeeID { get; set; }
    public EmployeeDTO Employee { get; set; }

    public DateTime ArrivalTime { get; set; }
    public DateTime? DepartureTime { get; set; }
}
