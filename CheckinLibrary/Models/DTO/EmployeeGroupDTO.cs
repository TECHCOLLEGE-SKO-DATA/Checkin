using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employeeGroup")]
public class EmployeeGroupDTO
{
    [Key]
    public int ID { get; set; }

    public int EmployeeID { get; set; }
    public EmployeeDTO Employee { get; set; }
    
    public int GroupID { get; set; }
    public GroupDTO Group { get; set; }
}
