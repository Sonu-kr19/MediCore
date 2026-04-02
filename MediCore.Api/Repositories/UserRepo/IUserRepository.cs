using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int id, User user);   
    Task<User?> GetUserByEmailAsync(string email);
    Task UpdatePasswordAsync(User user);
    Task RegisterUserAsync(User user);
}
