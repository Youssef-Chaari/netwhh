using BiProject.Api.Data.Oltp.Entities;

namespace BiProject.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> UserExistsAsync(string username, string email);
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
