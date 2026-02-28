using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.DTOs.Orders;
using BiProject.Api.Repositories.Interfaces;
using BiProject.Api.Services.Interfaces;

namespace BiProject.Api.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync();
            return orders.Select(MapToDto);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync();
            return orders.Where(o => o.UserId == userId).Select(MapToDto);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            if (order == null) return null;
            return MapToDto(order);
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto dto)
        {
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null) continue;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var createdOrder = await _orderRepository.GetOrderWithDetailsAsync(order.Id);
            return createdOrder != null ? MapToDto(createdOrder) : null;
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;

            order.Status = dto.Status;
            _orderRepository.Update(order);
            return await _orderRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;

            _orderRepository.Delete(order);
            return await _orderRepository.SaveChangesAsync();
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                UserName = order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : string.Empty,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product != null ? oi.Product.Name : string.Empty,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }
    }
}
