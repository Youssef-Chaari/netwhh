using System;
using System.Threading.Tasks;
using BiProject.Api.DTOs.Auth;
using BiProject.Api.Services.Implementations;
using BiProject.Api.Data.Oltp;
using BiProject.Api.Data.Oltp.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace BiProject.Tests
{
    public class AuthServiceTests : IDisposable
    {
        private readonly OltpDbContext _context;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<OltpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new OltpDbContext(options);

            _mockConfig = new Mock<IConfiguration>();
            // Setup secret key to avoid "Value cannot be null" in Encoding.UTF8.GetBytes
            _mockConfig.Setup(c => c["JwtSettings:SecretKey"]).Returns("SuperSecretKey123ForTestingPurposes!");
            _mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");

            _authService = new AuthService(_context, _mockConfig.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "User" };
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("GoodPassword123!");
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@domain.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = passwordHash,
                RoleId = 1,
                Role = role
            };

            await _context.Roles.AddAsync(role);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto { Username = "testuser", Password = "GoodPassword123!" };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var role = new Role { Id = 2, Name = "User" };
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("GoodPassword123!");
            var user = new User
            {
                Id = 2,
                Username = "testuser2",
                Email = "test2@domain.com",
                FirstName = "Jane",
                LastName = "Doe",
                PasswordHash = passwordHash,
                RoleId = 2,
                Role = role
            };

            await _context.Roles.AddAsync(role);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto { Username = "testuser2", Password = "BadPassword" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
            Assert.Equal("Données d'authentification invalides.", ex.Message);
        }

        [Fact]
        public async Task LoginAsync_WithUnknownUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "unknownuser", Password = "AnyPassword" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
            Assert.Equal("Données d'authentification invalides.", ex.Message);
        }
    }
}
