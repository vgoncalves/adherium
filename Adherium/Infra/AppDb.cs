using Adherium.Domain.Auth;
using Adherium.Domain.Onboarding;
using Adherium.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace Adherium.Infra;

public class AppDb : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Treatment> Treatments { get; set; }
    
    public DbSet<Event> TreatmentEvents { get; set; }
}