using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<StudentProject> StudentProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<Student>()
                .Property(s => s.IsBlocked)
                .HasDefaultValue(false);

           
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            // ✅ ProjectGroup - Tutor (One-to-Many)
            modelBuilder.Entity<ProjectGroup>()
                .HasOne(pg => pg.Tutor)
                .WithMany(t => t.TutoredGroups)
                .HasForeignKey(pg => pg.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Student - StudentProject (One-to-One)
            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentProjects)
                .WithOne(sp => sp.Student)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
