using Adherium.Domain.Auth;
using Adherium.Domain.Devices;
using Adherium.Infra;
using FastEndpoints;

namespace Adherium.Api.Features.Devices.SendEvents;

public class Endpoint(AppDb db, AuthorizationService authorizationService) : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/devices/{id}/events");
        Roles(nameof(UserRole.Patient));
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var user = User.Map();

        if(!await authorizationService.IsTreatmentOwnedByUser(request.TreatmentId, user.Id))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var events = request.Events
            .Select(e => new DeviceEvent
            {
                Id = e.EventId,
                TreatmentId = request.TreatmentId,
                Timestamp = e.Timestamp,
                Type = Enum.Parse<DeviceEventType>(e.EventType)
            });

        await db.TreatmentEvents.AddRangeAsync(events, ct);
        await db.SaveChangesAsync(ct);
        
        await Send.OkAsync(cancellation: ct);
    }
}