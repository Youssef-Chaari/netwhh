using BiProject.Api.Data.DataWarehouse;
using BiProject.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Repositories.Implementations
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly DwDbContext _context;

        public AnalyticsRepository(DwDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetSalesByPeriodAsync()
        {
            return await _context.FactInternetSales
                .AsNoTracking()
                .Include(f => f.OrderDate)
                .GroupBy(f => new { f.OrderDate!.CalendarYear, f.OrderDate!.MonthNumberOfYear, f.OrderDate!.EnglishMonthName })
                .Select(g => new
                {
                    Year = g.Key.CalendarYear,
                    Month = g.Key.MonthNumberOfYear,
                    MonthName = g.Key.EnglishMonthName,
                    TotalSales = g.Sum(f => f.SalesAmount),
                    TotalGrossMargin = g.Sum(f => f.GrossMargin ?? 0),
                    TotalCost = g.Sum(f => f.TotalCost ?? 0),
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync<dynamic>();
        }

        public async Task<IEnumerable<dynamic>> GetSalesByCustomerAsync()
        {
            return await _context.FactInternetSales
                .AsNoTracking()
                .Include(f => f.Customer)
                .GroupBy(f => new { f.CustomerSk, f.Customer!.FirstName, f.Customer!.LastName })
                .Select(g => new
                {
                    CustomerId = g.Key.CustomerSk,
                    CustomerName = g.Key.FirstName + " " + g.Key.LastName,
                    TotalSales = g.Sum(f => f.SalesAmount),
                    TotalGrossMargin = g.Sum(f => f.GrossMargin ?? 0),
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.TotalGrossMargin) // Sort by margin
                .Take(50) // Top 50 customers to avoid massive payloads
                .ToListAsync<dynamic>();
        }

        public async Task<IEnumerable<dynamic>> GetSalesByProductAsync()
        {
            return await _context.FactInternetSales
                .AsNoTracking()
                .Include(f => f.Product)
                .GroupBy(f => new { f.ProductSk, f.Product!.EnglishProductName })
                .Select(g => new
                {
                    ProductId = g.Key.ProductSk,
                    ProductName = g.Key.EnglishProductName,
                    TotalSales = g.Sum(f => f.SalesAmount),
                    TotalGrossMargin = g.Sum(f => f.GrossMargin ?? 0),
                    QuantitySold = g.Sum(f => f.OrderQuantity)
                })
                .OrderByDescending(x => x.TotalGrossMargin) // Rank by Profit instead of pure volume
                .Take(50)
                .ToListAsync<dynamic>();
        }

        public async Task<IEnumerable<dynamic>> GetSalesByCategoryAsync()
        {
            return await _context.FactInternetSales
                .AsNoTracking()
                .Include(f => f.Product)
                .Where(f => !string.IsNullOrEmpty(f.Product!.Color))
                .GroupBy(f => f.Product!.Color)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalSales = g.Sum(f => f.SalesAmount),
                    QuantitySold = g.Sum(f => f.OrderQuantity)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync<dynamic>();
        }

        public async Task<IEnumerable<dynamic>> GetSalesByChannelAsync()
        {
            return await _context.FactInternetSales
                .AsNoTracking()
                .GroupBy(f => f.OnlineOrderFlag)
                .Select(g => new
                {
                    Channel = g.Key == true ? "Online" : "Reseller",
                    TotalSales = g.Sum(f => f.SalesAmount),
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync<dynamic>();
        }

        public async Task<dynamic> GetKpisAsync()
        {
            var totalSales = await _context.FactInternetSales.AsNoTracking().SumAsync(f => f.SalesAmount);
            var totalGrossMargin = await _context.FactInternetSales.AsNoTracking().SumAsync(f => f.GrossMargin ?? 0);
            var totalOrders = await _context.FactInternetSales.AsNoTracking().CountAsync();
            var totalCustomers = await _context.FactInternetSales.AsNoTracking().Select(f => f.CustomerSk).Distinct().CountAsync();
            var totalProductsSold = await _context.FactInternetSales.AsNoTracking().SumAsync(f => f.OrderQuantity);

            // Compute aggregations manually
            var avgMarginRate = totalSales > 0 ? (totalGrossMargin / totalSales) * 100 : 0;
            var avgOrderValue = totalOrders > 0 ? (totalSales / totalOrders) : 0;

            return new
            {
                TotalSales = totalSales,
                TotalGrossMargin = totalGrossMargin,
                AverageMarginRate = avgMarginRate,
                AverageOrderValue = avgOrderValue,
                TotalOrders = totalOrders,
                TotalUniqueCustomers = totalCustomers,
                TotalProductsSold = totalProductsSold
            };
        }
    }
}
