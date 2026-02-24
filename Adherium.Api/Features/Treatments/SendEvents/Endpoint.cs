using Adherium.Api.Features.Treatment.SendUsageEvents;
using Adherium.Domain.Auth;
using Adherium.Domain.Treatments;
using Adherium.Infra;
using FastEndpoints;

namespace Adherium.Api.Features.Treatments.SendEvents;

public class Endpoint(AppDb db) : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/treatments/{id}/events");
        Roles(nameof(UserRole.Patient), nameof(UserRole.Admin));
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var patient = User.Map();
        
        
        
        // Map payload to domain events
        var domainEvents = request.Events
            .Select(e => new TreatmentEvent
            {
                Id = e.EventId,
                TreatmentId = e.TreatmentId,
                Timestamp = e.Timestamp,
                Type = Enum.Parse<EventType>(e.EventType)
            });

        await db.TreatmentEvents.AddRangeAsync(domainEvents, ct);
        await db.SaveChangesAsync(ct);
        
        await Send.OkAsync(cancellation: ct);
    }
}