using Adherium.Domain.Auth;
using Adherium.Domain.Devices;
using Adherium.Domain.Patients;
using Adherium.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;

namespace Adherium.Infra;

public class AppDb : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Prescription> Treatments { get; set; }
    
    public DbSet<DeviceEvent> TreatmentEvents { get; set; }
}