using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Absence")]
public class AbsenceDTO
{
    [Key]
    public virtual int ID { get; set; }

    public virtual int EmployeeId { get; set; }
    public virtual EmployeeDTO Employee { get; set; }

    public virtual DateTime FromDate { get; set; }
    public virtual DateTime ToDate { get; set; }
    public virtual string Note { get; set; }

    public virtual int AbsenceReasonId { get; set; }
}
