using System;

namespace MediCore.Api.Repositories.AuditRepo;

public interface IAuditLogRepository
{
    Task LogAsync(int? userId, string action, string resource);
}
