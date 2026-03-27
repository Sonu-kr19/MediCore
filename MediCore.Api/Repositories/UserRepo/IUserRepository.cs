using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int id, User user);
    Task<User?> GetUserByEmailAsync(string email);
}
