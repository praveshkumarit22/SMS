using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<StudentDocument> StudentDocuments => Set<StudentDocument>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.AdmissionNo)
                .IsUnique();

            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Section>().ToTable("Sections");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<StudentDocument>().ToTable("StudentDocuments");

            // Seed sample classes and sections
            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, Name = "Class 1" },
                new Class { Id = 2, Name = "Class 2" },
                new Class { Id = 3, Name = "Class 3" }
            );
            modelBuilder.Entity<Section>().HasData(
                new Section { Id = 1, ClassId = 1, Name = "A" },
                new Section { Id = 2, ClassId = 1, Name = "B" },
                new Section { Id = 3, ClassId = 2, Name = "A" }
            );
        }
    }
}
