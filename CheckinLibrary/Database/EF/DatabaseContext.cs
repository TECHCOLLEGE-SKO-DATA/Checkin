using Microsoft.EntityFrameworkCore;

public class CheckInDbContext : DbContext
{
    public CheckInDbContext(DbContextOptions<CheckInDbContext> options) : base(options) { }

    public DbSet<EmployeeDTO> Employees { get; set; }
    public DbSet<GroupDTO> Groups { get; set; }
    public DbSet<EmployeeGroupDTO> EmployeeGroups { get; set; }
    public DbSet<OnSiteTimeDTO> OnSiteTimes { get; set; }
    public DbSet<AdminUserDTO> AdminUsers { get; set; }
    public DbSet<AbsenceDTO> Absences { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<EmployeeGroupDTO>()
            .HasOne(e => e.Employee)
            .WithMany(e => e.EmployeeGroups)
            .HasForeignKey(e => e.EmployeeID);

        mb.Entity<EmployeeGroupDTO>()
            .HasOne(e => e.Group)
            .WithMany(g => g.EmployeeGroups)
            .HasForeignKey(e => e.GroupID);

        mb.Entity<OnSiteTimeDTO>()
            .HasOne(o => o.Employee)
            .WithMany(e => e.OnSiteTimes)
            .HasForeignKey(o => o.EmployeeID);

        mb.Entity<AbsenceDTO>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Absences)
            .HasForeignKey(a => a.EmployeeId);
    }
}
