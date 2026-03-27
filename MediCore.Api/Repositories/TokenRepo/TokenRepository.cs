using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.TokenRepo;

public class TokenRepository : ITokenRepository
{
    private readonly MediCoreDbContext _context;
    public TokenRepository(MediCoreDbContext context)
    {
        _context=context;
    }
    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Token==token);
        if (refreshToken == null)
        {
            return null;
        }
        return refreshToken;
    }
}
