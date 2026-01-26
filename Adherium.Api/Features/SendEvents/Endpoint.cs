using FastEndpoints;

namespace Adherium.Api.Features.SendEvents;

public class Endpoint(IEventRepository eventRepository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/events");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var patient = User.AsPatient();
        
        // Map payload to domain events
        var domainEvents = request.Events
            .Where(e => patient.DeviceIds.Contains(e.DeviceId))
            .Select(e => new Event(
                e.EventId,
                patient.Id,
                e.DeviceId,
                e.Timestamp,
                Enum.Parse<EventType>(e.EventType)
            ));

        var processedEventIds = eventRepository.SaveEvents(domainEvents);

        // Calculate adherence score
        var puffInhaledEvents = eventRepository.GetEvents(patient.Id, eventType: EventType.PuffInhaled).ToList();
        var adherenceScore = AdherenceCalculator.CalculateScore(puffInhaledEvents, DateTime.UtcNow);

        var response = new Response(adherenceScore, processedEventIds);

        await Send.OkAsync(response, ct);
    }
}