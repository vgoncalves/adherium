using System.Collections.Concurrent;
using Adherium.Api.Shared.Domain;

namespace Adherium.Api.Shared.Infra;

public class InMemoryRepository : IEventRepository, IPatientRepository
{
    private readonly ConcurrentDictionary<Guid, byte> _eventIds = new();
    private readonly ConcurrentBag<Event> _events = new();

    public IEnumerable<Guid> SaveEvents(IEnumerable<Event> events)
    {
        var processedEventIds = new List<Guid>();

        foreach (var @event in events)
        {
            if (_eventIds.TryAdd(@event.Id, 0))
            {
                _events.Add(@event);

                processedEventIds.Add(@event.Id);
            }
        }

        return processedEventIds;
    }

    public IEnumerable<Event> GetEvents(Guid? userId = null, EventType? eventType = null)
    {
        return _events.Where(e =>
            (!userId.HasValue || e.PatientId == userId.Value) &&
            (!eventType.HasValue || e.Type == eventType.Value));
    }
    
    #region

    private List<Patient> Patient { get; } = [SeedData.Patient];

    public Patient? GetPatient(string email, string password)
    {
        return Patient.FirstOrDefault(u => u.Email == email && u.Password == password); 
    }

    #endregion
}