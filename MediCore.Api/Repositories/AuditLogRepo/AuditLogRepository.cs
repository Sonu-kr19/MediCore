using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.AuditRepo;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly MediCoreDbContext _context;

    public AuditLogRepository(MediCoreDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(int? userId, string action, string resource)
    {
        var log = new AuditLog
        {
            UserID    = userId ?? 0,
            Action    = action,
            Resource  = resource,
            Timestamp = DateTime.UtcNow
        };

        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
