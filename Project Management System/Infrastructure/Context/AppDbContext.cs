using Domain.Enum;
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

           
            modelBuilder.Entity<Student>()
                .Property(s => s.IsBlocked)
                .HasDefaultValue(false);



            modelBuilder.Entity<ProjectGroup>(entity =>
            {
               
                entity.Property(e => e.Status)
                      .HasConversion<int>() 
                      .HasDefaultValue(ProjectStatus.Assigned);
            });

 
            modelBuilder.Entity<ProjectGroup>()
                .HasOne(pg => pg.Tutor)
                .WithMany(s => s.TutoredGroups)
                .HasForeignKey(pg => pg.TutorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

      
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(pg => pg.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

       
            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentProjects)
                .WithOne(sp => sp.Student)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

       
      
            modelBuilder.Entity<TutorReview>()
                .HasOne<ProjectGroup>()
                .WithMany()
                .HasForeignKey(tr => tr.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

     
            modelBuilder.Entity<TutorReview>()
                .HasOne(tr => tr.Tutor)
                .WithMany()
                .HasForeignKey(tr => tr.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

           
        }
    }
}
