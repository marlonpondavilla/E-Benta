using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set decimal precision for ProductPrice
            modelBuilder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasPrecision(18, 2);

            // Set decimal precision for CartItem.UnitPrice
            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
