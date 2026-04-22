using System;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.AppointmentRepository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly MediCoreDbContext _context;
    public AppointmentRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public async Task<bool> DoctorExists(int doctorId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == doctorId);
        if(user==null) return false;
        if (user.RoleName.ToString() == "Doctor")
        {
            return true;
        }
        return false;      
    }

    public async Task<List<Schedule>> GetFreeSlots(int doctorId, DateOnly date)
    {
        return await _context.Schedules.Where(
            s => s.DoctorID == doctorId && s.Date == date && s.Availability == true)
            .OrderBy(s => s.TimeSlot)
            .ToListAsync();
    }

    public async Task<Appointment> CreateAppointment(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        var schedule = await _context.Schedules.FirstOrDefaultAsync(a => a.DoctorID == appointment.DoctorID && a.Date == appointment.Date && a.TimeSlot == appointment.Time);
        schedule.Availability=false;
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment?> FindIdempotencyKey(string key)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(k=>k.IdempotencyKey==key);
        if(appointment==null) return null;
        return appointment;
    }
    
    public async Task<Appointment?> GetByIdAsync(int appointmentId)
    {
        return await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
    }
    
    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        var schedule = await _context.Schedules.FirstOrDefaultAsync(a => a.DoctorID == appointment.DoctorID && a.Date == appointment.Date && a.TimeSlot == appointment.Time);
        schedule.Availability=true;
        await _context.SaveChangesAsync();
    }

}
