using Adherium.Domain.Auth;
using Adherium.Domain.Devices;
using Adherium.Domain.Patients;
using Microsoft.EntityFrameworkCore;

namespace Adherium.Infra;

public class AppDb : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Device> Devices { get; set; }
    
    public DbSet<DeviceEvent> DeviceEvents { get; set; }
}