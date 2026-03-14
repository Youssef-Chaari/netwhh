using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace BiProject.Tests.Integration
{
    public class OrderControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrderControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Client simulé pour des requêtes complètes MVC sans démarrer un vrai serveur Kestrel
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetOrders_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Endpoint [Authorize] requiring Admin or User
            var response = await _client.GetAsync("/api/orders");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_WithoutAuthentication_ReturnsUnauthorized()
        {
            var content = new StringContent("{ \"productIds\": [1, 2] }", System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/orders", content);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Endpoint [Authorize(Roles = "Admin")]
            var content = new StringContent("{ \"name\": \"Test\", \"price\": 100 }", System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/products", content);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
