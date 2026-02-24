namespace Adherium.Domain.Treatments;

public class Treatment
{
    public Guid Id { get; set; }

    public Guid DeviceId { get; set; }

    public Guid PatientId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual List<TreatmentEvent> Events { get; set; } = [];
}