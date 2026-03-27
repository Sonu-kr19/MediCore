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
        List<User> users =  await _context.Users.ToListAsync();
            if (users.Count == 0)
            {
                throw new Exception(Utilities.ErrorMessages.User.UsersNotFound);
            }
        return users;
    }
}
