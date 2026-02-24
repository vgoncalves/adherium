using Adherium.Domain.Treatments;
using FastEndpoints;
using FluentValidation;

namespace Adherium.Api.Features.Treatments.SendEvents;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleForEach(x => x.Events).ChildRules(events =>
        {
            events.RuleFor(e => e.EventId).NotEmpty();
            events.RuleFor(e => e.DeviceId).NotEmpty();
            events.RuleFor(e => e.EventType)
                .NotEmpty()
                .Must(x => Enum.TryParse<EventType>(x, out _))
                .WithMessage("Invalid event type");
        });
    }
}