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
        public DbSet<TutorReview> TutorReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set default value for Student.IsBlocked
            modelBuilder.Entity<Student>()
                .Property(s => s.IsBlocked)
                .HasDefaultValue(false);

            // ProjectGroup → Tutor (Many-to-One)
            modelBuilder.Entity<ProjectGroup>()
                .HasOne(pg => pg.Tutor)
                .WithMany(s => s.TutoredGroups)
                .HasForeignKey(pg => pg.TutorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // ProjectGroup → Students (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(pg => pg.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Student → StudentProjects (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentProjects)
                .WithOne(sp => sp.Student)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentProject → TutorReviews (One-to-Many)
            modelBuilder.Entity<StudentProject>()
                .HasMany(sp => sp.TutorReviews)
                .WithOne(tr => tr.StudentProject)
                .HasForeignKey(tr => tr.StudentProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // TutorReview → Tutor (Student)
            modelBuilder.Entity<TutorReview>()
                .HasOne(tr => tr.Tutor)
                .WithMany()
                .HasForeignKey(tr => tr.TutorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade cycles
        }
    }
}
