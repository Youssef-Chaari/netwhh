using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using BiProject.Api.DTOs.Auth;
using System.Text.Json;
using System.Net.Http.Headers;

namespace BiProject.Tests.Integration
{
    public class AdminIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AdminIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_AsAdmin_ReturnsCreated()
        {
            // CT-15 : Création d'un produit par un Administrateur
            
            // 1. Inscription/Connexion pour obtenir un Token "Admin" (Seed)
            var regDto = new RegisterDto { 
                Username = "admin_ct15", 
                Password = "AdminPassword123!", 
                Email = "admin15@test.com",
                FirstName = "Admin",
                LastName = "System",
                RoleName = "Admin" 
            };
            
            // Tenter de créer le compte Admin (S'il existe déjà ça retournera 400 Bad Request, ce qui est OK)
            await _client.PostAsJsonAsync("/api/auth/register", regDto);
            
            // Récupération du JWT
            var loginDto = new LoginDto { Username = "admin_ct15", Password = "AdminPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            
            // S'assurer que le login a fonctionné
            loginResponse.EnsureSuccessStatusCode();

            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            // 2. Préparation du produit à créer
            var productDto = new { 
                ProductCode = "BIKE-ADMIN-CT15",
                Name = "Vélo de route Carbone Pro CT15",
                Price = 1499.99m,
                Description = "Créé dynamiquement par le test d'intégration CT-15.",
                CategoryId = 1
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", productDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
