using BiProject.Api.Data.DataWarehouse.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Data.DataWarehouse
{
    public class DwDbContext : DbContext
    {
        public DwDbContext(DbContextOptions<DwDbContext> options)
            : base(options)
        {
        }

        public DbSet<FactInternetSales> FactInternetSales { get; set; }
        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimProduct> DimProducts { get; set; }
        public DbSet<DimDate> DimDates { get; set; }
        public DbSet<DimGeography> DimGeographies { get; set; }
        public DbSet<DimSalesTerritory> DimSalesTerritories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("dw");

            modelBuilder.Entity<DimCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerSk).HasName("PK__DimCusto__A4AF97EBD12EF9F4");
                entity.ToTable("DimCustomer", "dw");
                entity.Property(e => e.CustomerSk).HasColumnName("CustomerSK");
            });

            modelBuilder.Entity<DimDate>(entity =>
            {
                entity.HasKey(e => e.DateSk).HasName("PK__DimDate__A4262D75A3FB564A");
                entity.ToTable("DimDate", "dw");
                entity.Property(e => e.DateSk).HasColumnName("DateSK");
            });

            modelBuilder.Entity<DimGeography>(entity =>
            {
                entity.HasKey(e => e.GeographySk).HasName("PK__DimGeogr__414FAF437D5B9446");
                entity.ToTable("DimGeography", "dw");
                entity.Property(e => e.GeographySk).HasColumnName("GeographySK");
            });

            modelBuilder.Entity<DimProduct>(entity =>
            {
                entity.HasKey(e => e.ProductSk).HasName("PK__DimProdu__B40D13B068832110");
                entity.ToTable("DimProduct", "dw");
                entity.Property(e => e.ProductSk).HasColumnName("ProductSK");
            });

            modelBuilder.Entity<DimSalesTerritory>(entity =>
            {
                entity.HasKey(e => e.SalesTerritorySk).HasName("PK__DimSales__332222052A565B2E");
                entity.ToTable("DimSalesTerritory", "dw");
                entity.Property(e => e.SalesTerritorySk).HasColumnName("SalesTerritorySK");
            });

            modelBuilder.Entity<FactInternetSales>(entity =>
            {
                entity.HasKey(e => e.FactSalesId).HasName("PK__FactInte__AA8B530747529860");
                entity.ToTable("FactInternetSales", "dw");

                entity.Property(e => e.CustomerSk).HasColumnName("CustomerSK");
                entity.Property(e => e.DateSk).HasColumnName("DateSK");
                entity.Property(e => e.GeographySk).HasColumnName("GeographySK");
                entity.Property(e => e.ProductSk).HasColumnName("ProductSK");
                entity.Property(e => e.SalesTerritorySk).HasColumnName("SalesTerritorySK");

                // Configure relationships for analytics queries
                entity.HasOne(f => f.Customer)
                    .WithMany(d => d.FactInternetSales)
                    .HasForeignKey(f => f.CustomerSk)
                    .HasPrincipalKey(d => d.CustomerSk);

                entity.HasOne(f => f.OrderDate)
                    .WithMany(d => d.FactInternetSales)
                    .HasForeignKey(f => f.DateSk)
                    .HasPrincipalKey(d => d.DateSk);

                entity.HasOne(f => f.Product)
                    .WithMany(d => d.FactInternetSales)
                    .HasForeignKey(f => f.ProductSk)
                    .HasPrincipalKey(d => d.ProductSk);

                entity.HasOne(f => f.Geography)
                    .WithMany(d => d.FactInternetSales)
                    .HasForeignKey(f => f.GeographySk)
                    .HasPrincipalKey(d => d.GeographySk);

                entity.HasOne(f => f.SalesTerritory)
                    .WithMany(d => d.FactInternetSales)
                    .HasForeignKey(f => f.SalesTerritorySk)
                    .HasPrincipalKey(d => d.SalesTerritorySk);
            });
        }
    }
}
