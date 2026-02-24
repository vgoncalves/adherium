namespace Adherium.Domain;

public interface IEventPublisher
{
    Task PublishEvent(object @event, CancellationToken cancellationToken = default);
}