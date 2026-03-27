using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.TokenRepo;

public interface ITokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token); 
}
