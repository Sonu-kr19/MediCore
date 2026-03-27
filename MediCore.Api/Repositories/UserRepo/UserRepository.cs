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
    public async Task RegisterUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(temp=>temp.Email==email);
        if (user == null)
        {
            return null;
        }
        return user;
    }
}
