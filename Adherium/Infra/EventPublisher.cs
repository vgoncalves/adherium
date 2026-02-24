namespace Adherium.Infra;

public class EventPublisher
{
    public Task PublishEvent(object @event, CancellationToken cancellationToken = default)
    {
        // Send event to service bus
        return Task.CompletedTask;
    }
}