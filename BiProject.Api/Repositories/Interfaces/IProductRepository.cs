using BiProject.Api.Data.Oltp.Entities;

namespace BiProject.Api.Repositories.Interfaces
{
    public interface IProductRepository : IOltpRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
        Task<Product?> GetProductWithDetailsAsync(int id);
    }
}
