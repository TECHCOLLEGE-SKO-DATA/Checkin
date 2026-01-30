using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("onSiteTime")]
public class OnSiteTimeDTO
{
    [Key]
    public virtual int Id { get; set; }

    public virtual int EmployeeID { get; set; }
    public virtual EmployeeDTO Employee { get; set; }

    public virtual DateTime ArrivalTime { get; set; }
    public virtual DateTime? DepartureTime { get; set; }
}
