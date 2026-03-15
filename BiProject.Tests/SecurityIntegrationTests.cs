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

        [Fact]
        public async Task GetKpis_AsUser_ReturnsForbidden()
        {
            // CT-19 : Limitation des privilèges Analytics (BI)
            
            // 1. S'authentifier en tant que User
            var loginDto = new LoginDto { Username = "test_user", Password = "TestPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            // 2. Tenter d'accéder aux Analytics
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/analytics/kpis");

            // 3. Vérifier le blocage (Attendu: Forbidden ou Unauthorized)
            // Note: Si le contrôleur n'a pas [Authorize(Roles="Admin")], cela retournera 200 et le test échouera.
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized, 
                $"Security Failure: User should not access Analytics. Received: {response.StatusCode}");
        }

        [Fact]
        public async Task GetOrder_Isolation_ReturnsForbidden_ForOtherUserOrder()
        {
            // CT-18 : Isolation des commandes (Sécurité anti-IDOR)
            
            // 1. User B (Victime) se connecte et crée une commande (Simulation par ID fixe)
            // Dans un test d'intégration idéal, on créerait une commande ici.
            // Pour CT-18, on vérifie que si on tente d'accéder à l'ID 1 avec le Token de 'test_user',
            // le système bloque l'accès si l'ID 1 appartient à un autre.
            
            var loginDto = new LoginDto { Username = "test_user", Password = "TestPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            // 2. Tente d'accéder à une commande (ID=1 par exemple)
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/orders/1");

            // 3. Assertion : Le système doit bloquer l'accès ou échouer sécuritairement si la faille existe (CT-20)
            // Si CT-20 a déjà montré la faille, CT-18 documente le test d'isolation.
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Unauthorized,
                $"Security Gap: IDOR possible on orders. Received: {response.StatusCode}");
        }
    }
}
