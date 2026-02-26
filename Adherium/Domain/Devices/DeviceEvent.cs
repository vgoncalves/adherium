
namespace Adherium.Domain.Devices;

public class DeviceEvent
{
    public required Guid Id { get; set; }

    public required DateTime Timestamp { get; set; }

    public required DeviceEventType Type { get; set; }

}