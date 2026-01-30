using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("group")]
public class GroupDTO
{
    [Key]
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual bool Isvisible { get; set; }

    public virtual ICollection<EmployeeGroupDTO> EmployeeGroups { get; set; } = new List<EmployeeGroupDTO>();
}
