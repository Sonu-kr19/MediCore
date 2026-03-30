using MediCore.Domain.Entities;

// namespace MediCore.Api.Repositories;
namespace MediCore.Api.Repositories.UserRepo;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int id, User user);   
    Task<User?> GetUserByEmailAsync(string email);
    Task RegisterUserAsync(User user);
    Task UpdatePasswordAsync(User user);
}
