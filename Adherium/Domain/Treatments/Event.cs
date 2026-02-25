namespace Adherium.Domain.Treatments;

public class Event
{
    public required Guid Id { get; set; }

    public required Guid TreatmentId { get; set; }

    public required DateTime Timestamp { get; set; }

    public required EventType Type { get; set; }

    public virtual Treatment Treatment { get; set; } = null!;
}