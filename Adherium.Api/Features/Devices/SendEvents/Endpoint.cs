using Adherium.Domain.Auth;
using Adherium.Domain.Devices;
using Adherium.Domain.Devices.Services;
using Adherium.Infra;
using FastEndpoints;

namespace Adherium.Api.Features.Devices.SendEvents;

public class Endpoint(AuthorizationService authorizationService, AppDb appDb) : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/devices/{id}/events");
        Roles(nameof(UserRole.Patient));
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var user = User.Map();

        if(!await authorizationService.IsDeviceAssignedToPatient(request.DeviceId, user.Id))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var events = request.DeviceEvents
            .Select(e => new DeviceEvent
            {
                Id = e.DeviceEventId,
                Timestamp = e.Timestamp,
                Type = Enum.Parse<DeviceEventType>(e.EventType)
            })
            .ToList();

        await appDb.DeviceEvents.AddRangeAsync(events, ct);
        await appDb.SaveChangesAsync(ct);
        
        await Send.OkAsync(cancellation: ct);
    }
}