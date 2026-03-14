using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using BiProject.Api.DTOs.Auth;
using System.Text.Json;

namespace BiProject.Tests.Integration
{
    public class SecurityIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SecurityIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostProduct_AsUser_ReturnsForbidden()
        {
            // CT-14 : Un utilisateur non admin ne peut pas créer un produit
            
            // 1. S'authentifier en tant que User (on utilise test_user existant)
            var loginDto = new LoginDto { Username = "test_user", Password = "TestPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            
            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                // Si l'utilisateur n'existe pas, on tente de le créer (robuste)
                var regDto = new RegisterDto { 
                    Username = "test_user_sec", 
                    Password = "TestPassword123!", 
                    Email = "sec@test.com",
                    FirstName = "Security",
                    LastName = "Test",
                    RoleName = "User"
                };
                await _client.PostAsJsonAsync("/api/auth/register", regDto);
                loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { Username = "test_user_sec", Password = "TestPassword123!" });
            }

            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            // 2. Tenter de créer un produit
            var productDto = new { 
                ProductCode = "FORBIDDEN-ITEM",
                Name = "Hacked Product",
                Price = 999.99,
                Description = "Should not be possible",
                CategoryId = 1
            };

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJsonAsync("/api/products", productDto);

            // 3. Vérifier le blocage (403 Forbidden)
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
