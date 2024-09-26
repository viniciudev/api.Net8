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
        //public DbSet<Cliente> Clientes { get; set; }
        //public DbSet<Produto> Produtos { get; set; }
        //public DbSet<ModeloProduto> Modelos { get; set; }
        //public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                //entity.HasMany(u => u.Clientes)
                //      .WithOne(c => c.User)
                //      .HasForeignKey(c => c.UserId)
                //      .OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("Client");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });

            //modelBuilder.Entity<Cliente>(entity =>
            //{
            //    // Removida a relação entre Cliente e Produto
            //});

            //modelBuilder.Entity<Produto>(entity =>
            //{
            //    entity.HasMany(p => p.Modelos)
            //          .WithOne(m => m.Produto)
            //          .HasForeignKey(m => m.ProdutoId)
            //          .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(p => p.Client)
            //          .WithMany(c => c.Produtos)
            //          .HasForeignKey(p => p.ClientId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

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
