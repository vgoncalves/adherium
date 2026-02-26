using Adherium.Infra;

namespace Adherium.Domain.Devices.Services;

public class DeviceService(AppDb appDb)
{
    public Task SaveEvents(List<DeviceEvent> events, CancellationToken ct = default)
    {
        appDb.DeviceEvents.AddRangeAsync(events, ct);
        return appDb.SaveChangesAsync(ct);
    }
}