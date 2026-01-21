using Microsoft.EntityFrameworkCore;
using StudentGrievancePortal.Models;

namespace StudentGrievancePortal.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grievance>Grievances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.ERP_Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Grievance>()
                .Property(g => g.TicketNumber)
                .HasComputedColumnSql("'GRV-' + CAST([GrievanceId] AS NVARCHAR(10))");

            modelBuilder.Entity<Grievance>()
                .Property(g => g.Status)
                .HasDefaultValue("Submitted");

            modelBuilder.Entity<Grievance>()
                .Property(g => g.Priority)
                .HasDefaultValue("Medium");

            modelBuilder.Entity<Grievance>()
                .Property(g => g.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Grievance>()
                .Property(g => g.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
        }



    }
}
