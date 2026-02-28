using BiProject.Api.Data.DataWarehouse.Entities;

namespace BiProject.Api.Repositories.Interfaces
{
    public interface IAnalyticsRepository
    {
        // Sales over time (e.g. by Year/Month)
        Task<IEnumerable<dynamic>> GetSalesByPeriodAsync();

        // Sales by customer
        Task<IEnumerable<dynamic>> GetSalesByCustomerAsync();

        // Sales by product categories
        Task<IEnumerable<dynamic>> GetSalesByProductAsync();

        // Sales by category
        Task<IEnumerable<dynamic>> GetSalesByCategoryAsync();

        // Sales by Channel
        Task<IEnumerable<dynamic>> GetSalesByChannelAsync();

        // Global KPIs
        Task<dynamic> GetKpisAsync();
    }
}
