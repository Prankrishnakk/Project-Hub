using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
              .Property(i => i.IsBlocked)
              .HasDefaultValue(false);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ProjectGroup>()
    .HasOne(pg => pg.Tutor)
    .WithMany(t => t.TutoredGroups)
    .HasForeignKey(pg => pg.TutorId)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

