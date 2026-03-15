using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using BiProject.Api.DTOs.Products;
using BiProject.Api.DTOs.Auth;
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
            
            // 1. Authentification (Nécessaire car [Authorize] est présent sur le contrôleur)
            var loginDto = new LoginDto { Username = "test_user", Password = "TestPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            
            // Si l'utilisateur n'existe pas, on tente de le créer (rendre le test autonome)
            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                var regDto = new RegisterDto { 
                    Username = "test_user", 
                    Password = "TestPassword123!", 
                    Email = "test_ct13@test.com",
                    FirstName = "CT13",
                    LastName = "Tester",
                    RoleName = "User"
                };
                await _client.PostAsJsonAsync("/api/auth/register", regDto);
                loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            }

            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. Récupérer tous les produits pour avoir un ID valide
            var productsResponse = await _client.GetAsync("/api/products");
            var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
            
            if (products == null || !products.Any())
            {
                // Si pas de produits, on échoue explicitement car le SUT n'est pas prêt
                Assert.Fail("Échec CT-13 : Aucun produit trouvé dans le catalogue pour effectuer le test de détail.");
                return;
            }

            var targetId = products.First().Id;

            // 3. Appeler l'endpoint de détail
            var response = await _client.GetAsync($"/api/products/{targetId}");

            // 4. Assertions
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal(targetId, product.Id);
            Assert.NotEmpty(product.Name);
        }
    }
}
