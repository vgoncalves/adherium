using Adherium.Domain.Devices;
using FastEndpoints;
using FluentValidation;

namespace Adherium.Api.Features.Devices.SendEvents;

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
                .Must(x => Enum.TryParse<DeviceEventType>(x, out _))
                .WithMessage("Invalid event type");
        });
    }
}