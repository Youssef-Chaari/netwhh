using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.DTOs.Products;
using BiProject.Api.Repositories.Interfaces;
using BiProject.Api.Services.Interfaces;

namespace BiProject.Api.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetProductsWithDetailsAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetProductWithDetailsAsync(id);
            if (product == null) return null;
            return MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                ProductCode = dto.ProductCode,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Description = string.IsNullOrWhiteSpace(dto.Description) 
                    ? null 
                    : new ProductDescription { Description = dto.Description }
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            // Fetch with details to get the category and description names
            var createdProduct = await _repository.GetProductWithDetailsAsync(product.Id);
            return MapToDto(createdProduct!);
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.ProductCode = dto.ProductCode;
            existing.Price = dto.Price;
            existing.CategoryId = dto.CategoryId;
            
            if (existing.Description != null)
            {
                existing.Description.Description = dto.Description;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                existing.Description = new ProductDescription { Description = dto.Description };
            }

            _repository.Update(existing);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _repository.Delete(existing);
            return await _repository.SaveChangesAsync();
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductCode = product.ProductCode,
                Price = product.Price,
                CategoryName = product.Category?.Name ?? string.Empty,
                Description = product.Description?.Description ?? string.Empty
            };
        }
    }
}
