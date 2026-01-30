using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("adminUser")]
public class AdminUserDTO
{
    [Key]
    public virtual int ID { get; set; }
    public virtual string Username { get; set; }
    public virtual string HashedPassword { get; set; }
}
