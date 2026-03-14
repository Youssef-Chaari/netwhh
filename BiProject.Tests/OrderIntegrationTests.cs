using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using BiProject.Api.DTOs.Auth;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace BiProject.Tests.Integration
{
    public class OrderControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrderControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetOrders_WithoutAuthentication_ReturnsUnauthorized()
        {
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
        public async Task CreateOrder_AsUser_ReturnsCreated()
        {
            // CT-17 : Passage d'une commande valide (Backend)
            var regDto = new RegisterDto { 
                Username = "user_ct17", 
                Password = "UserPassword123!", 
                Email = "user17@test.com",
                FirstName = "User",
                LastName = "Tester",
                RoleName = "User" 
            };
            await _client.PostAsJsonAsync("/api/auth/register", regDto);
            
            var loginDto = new LoginDto { Username = "user_ct17", Password = "UserPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();
            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Payload for creating an order
            var orderDto = new { ProductIds = new[] { 1 } };

            var response = await _client.PostAsJsonAsync("/api/orders", orderDto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetOrders_AsUser_ReturnsOk()
        {
            // CT-16 : Consultation d'historique (Backend)
            var regDto = new RegisterDto { 
                Username = "user_ct16", 
                Password = "UserPassword123!", 
                Email = "user16@test.com",
                FirstName = "User",
                LastName = "Tester",
                RoleName = "User" 
            };
            await _client.PostAsJsonAsync("/api/auth/register", regDto);
            
            var loginDto = new LoginDto { Username = "user_ct16", Password = "UserPassword123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();
            var authResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = authResult.GetProperty("token").GetString();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // On crée une commande pour être sûr d'avoir de la donnée
            await _client.PostAsJsonAsync("/api/orders", new { ProductIds = new[] { 1 } });

            var response = await _client.GetAsync("/api/orders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_WithoutAuthentication_ReturnsUnauthorized()
        {
            var content = new StringContent("{ \"name\": \"Test\", \"price\": 100 }", System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/products", content);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
