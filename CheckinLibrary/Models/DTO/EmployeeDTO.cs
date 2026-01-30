using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employee")]
public class EmployeeDTO
{
    [Key]
    public virtual int ID { get; set; }

    [Column(TypeName = "char(11)")]
    public virtual string CardID { get; set; }

    public virtual string? FirstName { get; set; }
    public virtual string? MiddleName { get; set; }
    public virtual string? LastName { get; set; }

    public virtual bool? IsOffSite { get; set; }
    public virtual DateTime? OffSiteUntil { get; set; }

    public virtual  ObservableCollection<OnSiteTimeDTO> OnSiteTimes { get; set; } = new ObservableCollection<OnSiteTimeDTO>();
    public virtual ObservableCollection<EmployeeGroupDTO> EmployeeGroups { get; set; } = new ObservableCollection<EmployeeGroupDTO>();
    public virtual ObservableCollection<AbsenceDTO> Absences { get; set; } = new ObservableCollection<AbsenceDTO>();
}


