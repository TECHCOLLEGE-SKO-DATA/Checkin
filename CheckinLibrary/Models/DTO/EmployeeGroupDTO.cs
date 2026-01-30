using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employeeGroup")]
public class EmployeeGroupDTO
{
    [Key]
    public virtual int ID { get; set; }

    public virtual int EmployeeID { get; set; }
    public virtual  EmployeeDTO Employee { get; set; }
    
    public virtual int GroupID { get; set; }
    public virtual GroupDTO Group { get; set; }
}
