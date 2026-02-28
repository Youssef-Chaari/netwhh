using BiProject.Api.DTOs.Clients;

namespace BiProject.Api.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto?> GetClientByIdAsync(int id);
        Task<ClientDto> CreateClientAsync(CreateClientDto dto);
        Task<bool> UpdateClientAsync(int id, UpdateClientDto dto);
        Task<bool> DeleteClientAsync(int id);
    }
}
