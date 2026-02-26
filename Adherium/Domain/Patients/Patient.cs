using Adherium.Domain.Auth;

namespace Adherium.Domain.Patients;

public class Patient : User
{
    public required DateTime BirthDate { get; set; }

    public required Guid DeviceId { get; set; }
}
