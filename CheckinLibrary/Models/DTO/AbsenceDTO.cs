using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Absence")]
public class AbsenceDTO
{
    [Key]
    public int ID { get; set; }

    public int EmployeeId { get; set; }
    public EmployeeDTO Employee { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Note { get; set; }

    public int AbsenceReasonId { get; set; }
}
