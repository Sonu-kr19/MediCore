
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task RegisterUserAsync(User user);
    }
}
