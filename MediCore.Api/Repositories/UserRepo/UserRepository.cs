using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Repositories;
using MediCore.Api.Utilities;

namespace MediCore.Api.Repositories.UserRepo;
public class UserRepository:IUserRepository
{
    
     // Receives the DbContext via DI so EF Core's lifetime management
    // (scoped per request) is respected — we never new up a context directly,
    // which would bypass connection pooling and change tracking.
  private readonly MediCoreDbContext _context;
    public UserRepository(MediCoreDbContext context)
    {
        _context = context;
    }

    // Looks up a single user by email address.
    // Returns null instead of throwing when no match is found, so callers
    // can decide what the absence means (duplicate-check vs login failure)
    // without catching exceptions for normal control flow.
    //
    // FirstOrDefaultAsync is used over SingleOrDefaultAsync because email
    // uniqueness is already enforced at the DB level — there can only ever
    // be one match, so the extra "check for a second row" that Single does
    // is an unnecessary round-trip overhead.
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(temp=>temp.Email==email);

        // Explicit null check kept for clarity even though returning the
        // variable directly would behave identically — makes the intent
        // (this method is allowed to return null) obvious to future readers.
    public async Task<User?> GetUserByIdAsync(int userId) // Implement the method to retrieve a user by their ID
    {
        var user= await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

        if (user == null)
        {
            return null;
        }
        return user;
    }
    
    // Persists a new User entity to the database.
    // Add() only stages the entity in the EF change tracker — nothing hits
    // the DB until SaveChangesAsync() is called, which wraps the INSERT
    // in a transaction and awaits the round-trip, keeping the method
    // truly async and avoiding thread blocking on I/O.
    public async Task RegisterUserAsync(User user)
    {
         _context.Users.Add(user);
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
