using BiProject.Api.Data.Oltp;
using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Repositories.Implementations
{
    public class OrderRepository : OltpRepository<Order>, IOrderRepository
    {
        public OrderRepository(OltpDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
