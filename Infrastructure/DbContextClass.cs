using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> contextOptions) : base(contextOptions)
        { }

        public DbSet<Client> Client { get; set; }
        public DbSet<User> User { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                
                entity.ToTable("Client");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Email).HasColumnType("character varying").IsRequired();
                entity.Property(u => u.Name).HasColumnType("character varying").IsRequired();
                entity.Property(u => u.Password).HasColumnType("character varying").IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
              .Entries()
              .Where(e => e.Entity is BaseModel && (
                      e.State == EntityState.Added
                      || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}
