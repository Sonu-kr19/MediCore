using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories;

public interface IUserRepository
{    
    Task<User?> GetUserByEmailAsync(string email);
    Task UpdatePasswordAsync(User user);
}
