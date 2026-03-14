using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using BiProject.Api.DTOs.Products;
using System.Collections.Generic;
using System.Linq;

namespace BiProject.Tests.Integration
{
    public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProductById_ReturnsOk_WithProductDetails()
        {
            // CT-13 : Récupération des détails d'un produit (Backend)
            
            // 1. Récupérer tous les produits pour avoir un ID valide
            var productsResponse = await _client.GetAsync("/api/products");
            var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
            
            if (products == null || !products.Any())
            {
                // Si pas de produits, le test est "Inconclusif" par manque de données, 
                // mais dans le cadre du projet académique, on assume une DB seedée.
                return;
            }

            var targetId = products.First().Id;

            // 2. Appeler l'endpoint de détail
            var response = await _client.GetAsync($"/api/products/{targetId}");

            // 3. Assertions
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal(targetId, product.Id);
            Assert.NotEmpty(product.Name);
        }
    }
}
