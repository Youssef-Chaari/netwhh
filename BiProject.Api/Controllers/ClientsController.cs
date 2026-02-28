using BiProject.Api.DTOs.Clients;
using BiProject.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> List()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Detail(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound(new { message = "Client introuvable." });
            return Ok(client);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
        {
            var client = await _clientService.CreateClientAsync(dto);
            return CreatedAtAction(nameof(Detail), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto dto)
        {
            var updated = await _clientService.UpdateClientAsync(id, dto);
            if (!updated) return NotFound(new { message = "Client introuvable." });
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _clientService.DeleteClientAsync(id);
            if (!deleted) return NotFound(new { message = "Client introuvable." });

            return NoContent();
        }
    }
}
