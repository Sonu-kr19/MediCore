using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Repositories;
using MediCore.Api.Utilities;

namespace MediCore.Api.Repositories.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly MediCoreDbContext _context;
    public UserRepository(MediCoreDbContext context)
    {
        _context=context;
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
     public async Task<List<User>> GetAllUsersAsync()
    {
        List<User> users =  await _context.Users.ToListAsync();
            if (users.Count == 0)
            {
                throw new Exception(Utilities.ErrorMessages.UsersNotFound);
            }
        return users;
    }
    public async Task UpdatePasswordAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
     }
}
