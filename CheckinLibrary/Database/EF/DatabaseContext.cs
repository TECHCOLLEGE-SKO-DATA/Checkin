using Microsoft.EntityFrameworkCore;
using CheckinLibrary.Models;

namespace CheckinLibrary.Database.EF
{
    public class CheckInDbContext : DbContext
    {
        public CheckInDbContext(DbContextOptions<CheckInDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<EmployeeGroup> EmployeeGroups { get; set; }
        public DbSet<OnSiteTime> OnSiteTimes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Absence> Absences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee
            modelBuilder.Entity<Employee>()
                .ToTable("employee")
                .HasKey(e => e.ID);

            modelBuilder.Entity<Employee>()
                .Property(e => e.CardID)
                .HasColumnType("char(11)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsOffSite)
                .HasDefaultValue(false);

            // Group
            modelBuilder.Entity<Group>()
                .ToTable("group")
                .HasKey(g => g.ID);

            modelBuilder.Entity<Group>()
                .Property(g => g.Isvisible)
                .HasDefaultValue(false);

            // EmployeeGroup
            modelBuilder.Entity<EmployeeGroup>()
                .ToTable("employeeGroup")
                .HasKey(eg => eg.ID);

            modelBuilder.Entity<EmployeeGroup>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(eg => eg.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeGroup>()
                .HasOne<Group>()
                .WithMany()
                .HasForeignKey(eg => eg.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            // OnSiteTime
            modelBuilder.Entity<OnSiteTime>()
                .ToTable("onSiteTime")
                .HasKey(ot => ot.Id);

            modelBuilder.Entity<OnSiteTime>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(ot => ot.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);

            // AdminUser
            modelBuilder.Entity<AdminUser>()
                .ToTable("adminUser")
                .HasKey(a => a.ID);

            // Absence
            modelBuilder.Entity<Absence>()
                .ToTable("Absence")
                .HasKey(a => a.ID);

            modelBuilder.Entity<Absence>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class EmployeeGroup
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int GroupID { get; set; }
    }
}