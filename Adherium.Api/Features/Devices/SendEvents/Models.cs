namespace Adherium.Api.Features.Devices.SendEvents;

public record Request(Guid DeviceId, List<Request.DeviceEvent> DeviceEvents)
{
    public record DeviceEvent(
        Guid DeviceEventId,
        DateTime Timestamp,
        string EventType
    );
}

public record Response(
    IEnumerable<Guid> ProcessedEventIds
);