using BiProject.Api.DTOs.Orders;

namespace BiProject.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<OrderDto?> CreateOrderAsync(CreateOrderDto dto);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto);
        Task<bool> DeleteOrderAsync(int id);
    }
}
