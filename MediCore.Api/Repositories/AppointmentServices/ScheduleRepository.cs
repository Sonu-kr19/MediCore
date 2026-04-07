using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.AppointmentServices;

public class ScheduleRepository : IScheduleRepository
{
    private readonly MediCoreDbContext _context;
    public ScheduleRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public async Task<bool> DoctorExists(int doctorId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(d => d.UserID == doctorId);
        string roleName = user.RoleName.ToString();
        if (roleName == "Doctor")
            return true;
        else
            return false;
    }

    public async Task<List<Schedule>> GetFreeSlots(int doctorId, DateOnly date)
    {
        return await _context.Schedules.Where(
            s => s.DoctorID == doctorId && s.Date == date && s.Availability == true)
            .OrderBy(s => s.TimeSlot)
            .ToListAsync();
    }
}
