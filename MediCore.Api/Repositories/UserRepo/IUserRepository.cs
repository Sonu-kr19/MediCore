using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.UserRepo
{
    public interface IUserRepository
    {
      Task<User?> GetUserByEmailAsync(string email);
      Task RegisterUserAsync(User user);
    }
}
