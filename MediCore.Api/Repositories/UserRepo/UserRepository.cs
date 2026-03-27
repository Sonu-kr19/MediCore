
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly MediCoreDbContext _context;

        // Injects the EF Core DbContext via constructor injection so the repository
        // can interact with the database while remaining decoupled from the
        // concrete context creation — making it easier to mock in unit tests.
        public UserRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        // Looks up a single user by their email address from the database.
        // Returns null instead of throwing if no match is found, so the caller
        // (typically the service layer) can decide how to handle the missing user —
        // e.g., treat it as "available for registration" or "not found" without
        // needing to catch an exception.
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        // Persists a newly created User entity to the database.
        // SaveChangesAsync is called here (not in the service layer) because
        // this repository owns the unit of work for single-entity operations,
        // ensuring the insert is committed immediately after being staged.
        public async Task RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}