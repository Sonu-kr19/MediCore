using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly MediCoreDbContext _context;
    public UserRepository(MediCoreDbContext context)
    {
        _context=context;
    }
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
