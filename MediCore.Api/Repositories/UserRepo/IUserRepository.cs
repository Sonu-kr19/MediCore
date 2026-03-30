using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task UpdatePasswordAsync(User user);
}
