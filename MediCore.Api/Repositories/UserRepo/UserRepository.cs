using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.UserRepo;

public class UserRepository : IUserRepository
{
    MediCoreDbContext context = new MediCoreDbContext();
    public Task<List<User>> GetAllUsersAsync()
    {
        return context.Users.ToListAsync();
    }
}
