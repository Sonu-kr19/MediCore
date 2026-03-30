using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Repositories;
using MediCore.Api.Utilities;

namespace MediCore.Api.Repositories.UserRepo;
public class UserRepository:IUserRepository
{
  private readonly MediCoreDbContext _context;
    public UserRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByIdAsync(int userId) // Implement the method to retrieve a user by their ID
    {
        var user= await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

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
    public async Task UpdateUserAsync(int id,User user) // Implement the method to update an existing user's information
    {
        var existingUser = await GetUserByIdAsync(id);
        if (existingUser == null){
            throw new Exception("User not found.");
        }
        existingUser.Name = user.Name;
        existingUser.RoleName = user.RoleName;   
        existingUser.Phone = user.Phone;
        existingUser.Status = user.Status;
        await _context.SaveChangesAsync();
    }
    public async Task UpdatePasswordAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
     }
}
