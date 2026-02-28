using BiProject.Api.Data.Oltp;
using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Repositories.Implementations
{
    public class ProductRepository : OltpRepository<Product>, IProductRepository
    {
        public ProductRepository(OltpDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Description)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Description)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
