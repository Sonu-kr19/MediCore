using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task UpdateAsync(User user);
}
