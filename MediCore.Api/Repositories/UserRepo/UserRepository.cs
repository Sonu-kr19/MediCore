using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly MediCoreDbContext _context;
 
        public UserRepository(MediCoreDbContext context)
        {
            _context = context;
        }
 
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
 
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
 