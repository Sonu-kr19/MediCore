using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.AppointmentRepository;

public class ScheduleRepository : IScheduleRepository
{
    private readonly MediCoreDbContext _context;
    public ScheduleRepository(MediCoreDbContext context)
    {
        _context=context;
    }
    public async Task<Schedule?> GetSlotAsync(int doctorId, DateOnly date, TimeOnly timeSlot)
    {
        return await _context.Schedules.FirstOrDefaultAsync(s => s.DoctorID == doctorId && s.Date == date && s.TimeSlot == timeSlot && s.Availability == true);
    }
    public async Task<Schedule?> GetAllotedSlotAsync(int doctorId, DateOnly date, TimeOnly timeSlot)
    {
        return await _context.Schedules.FirstOrDefaultAsync(s => s.DoctorID == doctorId && s.Date == date && s.TimeSlot == timeSlot && s.Availability == false);
    }
    public async Task UpdateAsync(Schedule schedule)
    {
        _context.Schedules.Update(schedule);
        await _context.SaveChangesAsync();
    }
}
