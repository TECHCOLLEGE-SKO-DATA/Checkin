using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("adminUser")]
public class AdminUserDTO
{
    [Key]
    public int ID { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
}
