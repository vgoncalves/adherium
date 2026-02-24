namespace Adherium.Api.Features.Treatment.SendUsageEvents;

public record Request(List<Request.Event> Events)
{
    public record Event(
        Guid TreatmentId,
        Guid EventId,
        Guid DeviceId, 
        DateTime Timestamp,
        
        string EventType
    );
}

public record Response(
    IEnumerable<Guid> ProcessedEventIds
);