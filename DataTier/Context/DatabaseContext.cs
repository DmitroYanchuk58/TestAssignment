using DataTier.Entities;
using Microsoft.EntityFrameworkCore;
using Task = DataTier.Entities.Task;

namespace DataTier.Context
{
    public class DatabaseContext:DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Task> Tasks { get; set; }

        public DatabaseContext() : base()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-CPI51I3\MSSQLSERVER01;Initial Catalog=TestDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.Username)
                    .IsUnique();
                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.Username)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                entity.Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();

                entity.Property(u => u.UpdatedAt)
                .ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                .IsRequired();
            });

            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.IdUser);
        }
    }
}
