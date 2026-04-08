using System;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Domain.Entities;

public class MediCoreDbContext : DbContext
{
    public MediCoreDbContext(){}
    public MediCoreDbContext(DbContextOptions<MediCoreDbContext> options) : base(options){}
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    public virtual DbSet<Bill> Bills { get; set; }
    public virtual DbSet<BillItem> BillItems {get; set;}
    public virtual DbSet<ComplianceRecord> ComplianceRecords {get; set;}
    public virtual DbSet<Dispense> Dispenses {get; set;}
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<EMR> EMRs { get; set; }
    public virtual DbSet<FinanceOfficer> FinanceOfficers {get; set;}
    public virtual DbSet<InsuranceClaim> InsuranceClaims {get; set;}
    public virtual DbSet<LabReport> LabReports  { get; set; }
    public virtual DbSet<LabTest> LabTests { get; set; }
    public virtual DbSet<Medicine> Medicines {get; set;}
    public virtual DbSet<Nurse> Nurses {get; set;}
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<PatientDocument> PatientDocuments { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Pharmacist> Pharmacists { get; set; }
    public virtual DbSet<Prescription> Prescriptions { get; set; }
    public virtual DbSet<PrescriptionItem> PrescriptionItems { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Technician> Technicians { get; set; }
    public virtual DbSet<TreatmentLog> TreatmentLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<User>()
            .Property(u => u.RoleName)
            .HasConversion<string>();

        modelBuilder.Entity<Bill>()
            .HasOne(b=>b.Patient)
            .WithMany()
            .HasForeignKey(b=>b.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<LabTest>()
            .HasOne(l => l.DoctorIDNavigator)
            .WithMany(d => d.LabTests)
            .HasForeignKey(l => l.DoctorID)
            .OnDelete(DeleteBehavior.Cascade); // keep cascade here

        modelBuilder.Entity<LabTest>()
            .HasOne(l => l.PatientIDNavigator)
            .WithMany(p => p.LabTests)
            .HasForeignKey(l => l.PatientID)
            .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

        modelBuilder.Entity<LabTest>()
            .HasOne(l => l.TechnicianIDNavigator)
            .WithMany(t => t.LabTests)
            .HasForeignKey(l => l.TechnicianID)
            .OnDelete(DeleteBehavior.NoAction); // optional, default is NoAction

        modelBuilder.Entity<Appointment>()
            .HasOne(a=>a.PatientIDNavigator)
            .WithMany(p=>p.Appointments)
            .HasForeignKey(a=>a.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a=>a.Doctor)
            .WithMany(d=>d.Appointments)
            .HasForeignKey(a=>a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .Property(a=>a.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);
        
        modelBuilder.Entity<Appointment>()
            .HasIndex(a=>a.IdempotencyKey)
            .IsUnique();

        modelBuilder.Entity<EMR>()
            .HasOne(e => e.DoctorIDNavigator)
            .WithMany(d => d.EMRs)
            .HasForeignKey(e=>e.DoctorID)
            .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.SetNull
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.DoctorIDNavigator)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DoctorID)
            .OnDelete(DeleteBehavior.Restrict); // or SetNull

        modelBuilder.Entity<Schedule>()
        .HasOne(s => s.Doctor)      
        .WithMany()                 
        .HasForeignKey(s => s.DoctorID)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
