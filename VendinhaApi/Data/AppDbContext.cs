using Microsoft.EntityFrameworkCore;
using VendinhaApi.Models;

namespace VendinhaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Debt> Debts => Set<Debt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Cpf)
                .IsUnique();

            modelBuilder.Entity<Debt>()
                .Property(d => d.Value)
                .HasColumnType("decimal(18,2)");
        }
    }
}