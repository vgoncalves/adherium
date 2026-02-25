using Adherium.Domain.Devices;

namespace Adherium.Domain.Prescriptions;

public class Prescription
{
    public Guid Id { get; set; }

    public Guid DeviceId { get; set; }

    public Guid PatientId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual List<DeviceEvent> Events { get; set; } = [];
}