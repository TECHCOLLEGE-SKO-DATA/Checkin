using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("group")]
public class GroupDTO
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public bool Isvisible { get; set; }

    public ICollection<EmployeeGroupDTO> EmployeeGroups { get; set; } = new List<EmployeeGroupDTO>();
}
