using BiProject.Api.DTOs.Analytics;

namespace BiProject.Api.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync();
        Task<IEnumerable<SalesByChannelDto>> GetSalesByChannelAsync();
        Task<IEnumerable<SalesByCustomerDto>> GetSalesByCustomerAsync();
        Task<IEnumerable<SalesByProductDto>> GetSalesByProductAsync();
        Task<IEnumerable<SalesByCategoryDto>> GetSalesByCategoryAsync();
        Task<KpiDto> GetKpisAsync();
    }
}
