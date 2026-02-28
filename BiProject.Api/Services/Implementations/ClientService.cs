using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.DTOs.Clients;
using BiProject.Api.Repositories.Interfaces;
using BiProject.Api.Services.Interfaces;

namespace BiProject.Api.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _repository.GetAllAsync();
            return clients.Select(MapToDto);
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            return client != null ? MapToDto(client) : null;
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto dto)
        {
            var client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Company = dto.Company,
                Phone = dto.Phone
            };

            await _repository.AddAsync(client);
            await _repository.SaveChangesAsync();

            return MapToDto(client);
        }

        public async Task<bool> UpdateClientAsync(int id, UpdateClientDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.Company = dto.Company;
            existing.Phone = dto.Phone;

            _repository.Update(existing);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _repository.Delete(existing);
            return await _repository.SaveChangesAsync();
        }

        private static ClientDto MapToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Company = client.Company,
                Phone = client.Phone,
                CreatedAt = client.CreatedAt
            };
        }
    }
}
