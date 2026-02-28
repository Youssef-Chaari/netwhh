using BiProject.Api.DTOs.Analytics;
using BiProject.Api.Repositories.Interfaces;
using BiProject.Api.Services.Interfaces;

namespace BiProject.Api.Services.Implementations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _repository;

        public AnalyticsService(IAnalyticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync()
        {
            var data = await _repository.GetSalesByPeriodAsync();

            var result = new List<SalesByPeriodDto>();
            foreach (var item in data)
            {
                result.Add(new SalesByPeriodDto
                {
                    Year = (int)item.Year,
                    Month = (int)item.Month,
                    MonthName = (string)item.MonthName,
                    TotalSales = (decimal)item.TotalSales,
                    TotalGrossMargin = (decimal)item.TotalGrossMargin,
                    TotalCost = (decimal)item.TotalCost,
                    OrderCount = (int)item.OrderCount
                });
            }

            return result;
        }

        public async Task<IEnumerable<SalesByChannelDto>> GetSalesByChannelAsync()
        {
            var data = await _repository.GetSalesByChannelAsync();

            var result = new List<SalesByChannelDto>();
            foreach (var item in data)
            {
                result.Add(new SalesByChannelDto
                {
                    Channel = (string)item.Channel,
                    TotalSales = (decimal)item.TotalSales,
                    OrderCount = (int)item.OrderCount
                });
            }

            return result;
        }

        public async Task<IEnumerable<SalesByCustomerDto>> GetSalesByCustomerAsync()
        {
            var data = await _repository.GetSalesByCustomerAsync();

            var result = new List<SalesByCustomerDto>();
            foreach (var item in data)
            {
                result.Add(new SalesByCustomerDto
                {
                    CustomerId = (int)item.CustomerId,
                    CustomerName = (string)item.CustomerName,
                    TotalSales = (decimal)item.TotalSales,
                    TotalGrossMargin = (decimal)item.TotalGrossMargin,
                    OrderCount = (int)item.OrderCount
                });
            }

            return result;
        }

        public async Task<IEnumerable<SalesByProductDto>> GetSalesByProductAsync()
        {
            var data = await _repository.GetSalesByProductAsync();

            var result = new List<SalesByProductDto>();
            foreach (var item in data)
            {
                result.Add(new SalesByProductDto
                {
                    ProductId = (int)item.ProductId,
                    ProductName = (string)item.ProductName,
                    TotalSales = (decimal)item.TotalSales,
                    TotalGrossMargin = (decimal)item.TotalGrossMargin,
                    QuantitySold = (int)item.QuantitySold
                });
            }

            return result;
        }

        public async Task<IEnumerable<SalesByCategoryDto>> GetSalesByCategoryAsync()
        {
            var data = await _repository.GetSalesByCategoryAsync();

            var result = new List<SalesByCategoryDto>();
            foreach (var item in data)
            {
                result.Add(new SalesByCategoryDto
                {
                    CategoryName = (string)item.CategoryName,
                    TotalSales = (decimal)item.TotalSales,
                    QuantitySold = (int)item.QuantitySold
                });
            }

            return result;
        }

        public async Task<KpiDto> GetKpisAsync()
        {
            var kpis = await _repository.GetKpisAsync();

            return new KpiDto
            {
                TotalSales = (decimal)kpis.TotalSales,
                TotalGrossMargin = (decimal)kpis.TotalGrossMargin,
                AverageMarginRate = (decimal)kpis.AverageMarginRate,
                AverageOrderValue = (decimal)kpis.AverageOrderValue,
                TotalOrders = (int)kpis.TotalOrders,
                TotalUniqueCustomers = (int)kpis.TotalUniqueCustomers,
                TotalProductsSold = (int)kpis.TotalProductsSold
            };
        }
    }
}
