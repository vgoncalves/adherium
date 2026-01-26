namespace Adherium.Api.Features.SendEvents;

public record Request(List<Request.Event> Events)
{
    public record Event(
        Guid EventId,
        DateTime Timestamp,
        Guid DeviceId, 
        string EventType
    );
}

public record Response(
    decimal AdherenceScore,
    IEnumerable<Guid> ProcessedEventIds
);