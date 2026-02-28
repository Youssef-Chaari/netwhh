using System.ComponentModel.DataAnnotations;

namespace BiProject.Api.DTOs.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateClientDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Company { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateClientDto : CreateClientDto
    {
    }
}
