using BiProject.Api.Data.Oltp.Entities;

namespace BiProject.Api.Repositories.Interfaces
{
    public interface IOrderRepository : IOltpRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithDetailsAsync();
        Task<Order?> GetOrderWithDetailsAsync(int id);
    }
}
