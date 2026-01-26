namespace Adherium.Api.Shared.Domain;

public interface IEventRepository
{
    /// <returns>The processed (saved) event ids. Events may be duplicates and will be skipped.</returns>
    IEnumerable<Guid> SaveEvents(IEnumerable<Event> events);

    IEnumerable<Event> GetEvents(Guid? userId = null, EventType? eventType = null);
}
