using BiProject.Api.Data.Oltp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Data.Oltp
{
    public class OltpDbContext : DbContext
    {
        public OltpDbContext(DbContextOptions<OltpDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding default roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seeding default categories
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = 1, Name = "Bikes" },
                new ProductCategory { Id = 2, Name = "Components" },
                new ProductCategory { Id = 3, Name = "Clothing" },
                new ProductCategory { Id = 4, Name = "Accessories" }
            );

            // Seeding default description
            modelBuilder.Entity<ProductDescription>().HasData(
                new ProductDescription { Id = 1, Description = "High-performance carbon fiber frame." },
                new ProductDescription { Id = 2, Description = "Durable mountain bike with suspension." },
                new ProductDescription { Id = 3, Description = "Aerodynamic racing helmet." },
                new ProductDescription { Id = 4, Description = "Replacement chain for 11-speed drivetrain." }
            );

            // Seeding default products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Road Bike X-Speed", ProductCode = "RB-X1", Price = 1200.00m, CategoryId = 1, DescriptionId = 1 },
                new Product { Id = 2, Name = "Mountain Master", ProductCode = "MM-01", Price = 950.00m, CategoryId = 1, DescriptionId = 2 },
                new Product { Id = 3, Name = "Aero Helmet V2", ProductCode = "HT-A2", Price = 120.00m, CategoryId = 4, DescriptionId = 3 },
                new Product { Id = 4, Name = "Shimano Chain 11s", ProductCode = "CH-S11", Price = 35.00m, CategoryId = 2, DescriptionId = 4 }
            );

            // Relationships configuration
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Description)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DescriptionId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}
