namespace Adherium.Api.Shared.Domain;

public record Event(
    Guid Id,
    Guid PatientId,   
    Guid DeviceId,
    DateTime Timestamp,
    EventType Type
);
