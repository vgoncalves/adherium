namespace Adherium.Api.Features.Devices.SendEvents;

public record Request(Guid TreatmentId, List<Request.Event> Events)
{
    public record Event(
        Guid EventId,
        Guid DeviceId, 
        DateTime Timestamp,
        string EventType
    );
}

public record Response(
    IEnumerable<Guid> ProcessedEventIds
);