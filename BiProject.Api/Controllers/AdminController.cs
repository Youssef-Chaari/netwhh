using BiProject.Api.Data.Oltp.Entities;
using BiProject.Api.DTOs.Users;
using BiProject.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiProject.Api.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var dtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role?.Name ?? "User"
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role?.Name ?? "User"
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Username, dto.Email))
            {
                return BadRequest(new { message = "Username or Email already exists." });
            }

            var role = await _userRepository.GetRoleByNameAsync(dto.RoleName);
            if (role == null)
            {
                return BadRequest(new { message = "Role not found." });
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = role.Id,
                Role = role
            };

            await _userRepository.CreateUserAsync(user);

            var responseDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role.Name
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var role = await _userRepository.GetRoleByNameAsync(dto.RoleName);
            if (role == null)
            {
                return BadRequest(new { message = "Role not found." });
            }

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.RoleId = role.Id;
            user.Role = role;

            await _userRepository.UpdateUserAsync(user);

            var responseDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role.Name
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
                
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null && currentUserId == id.ToString())
            {
                return BadRequest(new { message = "An Admin cannot delete themselves." });
            }

            await _userRepository.DeleteUserAsync(user);
            return NoContent();
        }
    }
}
