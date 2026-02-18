using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data
{
    public class LearningDbContext(DbContextOptions<LearningDbContext> options) : DbContext(options)
    {
        public DbSet<Clients> clients { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Transactions> transactions { get; set; }
        public DbSet<Workers> workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>().HasKey(c => c.Id);
            modelBuilder.Entity<Clients>().Property(c => c.Id).ValueGeneratedOnAdd();///нужно добавить
            modelBuilder.Entity<Clients>().Property(c => c.FullName).HasMaxLength(100);
            modelBuilder.Entity<Clients>()
                .HasMany(c => c.transactions)
                .WithOne(t => t.clients)
                .HasForeignKey(t => t.ClientId);

            modelBuilder.Entity<Workers>().HasKey(w => w.Id);
            modelBuilder.Entity<Workers>().Property(w => w.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Workers>().Property(w => w.FullName).HasMaxLength(100);
            modelBuilder.Entity<Workers>().Property(w => w.Post).HasMaxLength(70);
            modelBuilder.Entity<Workers>()
                .HasMany(w => w.transactions)
                .WithOne(t => t.workers)
                .HasForeignKey(t => t.WorkerId);

            modelBuilder.Entity<Products>().HasKey(p => p.Id);
            modelBuilder.Entity<Products>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Products>().Property(p => p.Name).HasMaxLength(70);
            modelBuilder.Entity<Products>().Property(p => p.Category).HasMaxLength(70);
            modelBuilder.Entity<Products>().Property(p => p.Supplier).HasMaxLength(70);
            modelBuilder.Entity<Products>().ToTable(p => p.HasCheckConstraint("CheckCount", "Quantity > 0"));
            modelBuilder.Entity<Products>().ToTable(p => p.HasCheckConstraint("ValidPrice", "Price > 0"));
            modelBuilder.Entity<Products>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Products>()
                .HasMany(p => p.transactions)
                .WithMany(t => t.products);

            modelBuilder.Entity<Transactions>().HasKey(t => t.Id);
            modelBuilder.Entity<Transactions>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Transactions>().ToTable(t => t.HasCheckConstraint("CheckCount", "Quantity > 0"));
            modelBuilder.Entity<Transactions>().Property(t => t.Sum).HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
