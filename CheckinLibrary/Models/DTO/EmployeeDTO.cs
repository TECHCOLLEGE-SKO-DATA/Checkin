using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employee")]
public class EmployeeDTO
{
    [Key]
    public int ID { get; set; }

    [Column(TypeName = "char(11)")]
    public string CardID { get; set; }

    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }

    public bool IsOffSite { get; set; }
    public DateTime? OffSiteUntil { get; set; }

    public ICollection<OnSiteTimeDTO> OnSiteTimes { get; set; } = new List<OnSiteTimeDTO>();
    public ICollection<EmployeeGroupDTO> EmployeeGroups { get; set; } = new List<EmployeeGroupDTO>();
    public ICollection<AbsenceDTO> Absences { get; set; } = new List<AbsenceDTO>();
}
