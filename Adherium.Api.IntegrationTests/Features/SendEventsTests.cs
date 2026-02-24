using System.Net;
using Adherium.Api.Features.Treatment.SendUsageEvents;
using Adherium.Domain.Treatments;
using Adherium.Infra;
using FastEndpoints;
using Shouldly;

namespace Adherium.Api.IntegrationTests.Features;

public class SendEventsTests(App app) : IClassFixture<App>
{
    [Fact]
    public async Task SendEvents_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var request = new Request([
            new Request.Event(Guid.NewGuid(), DateTime.UtcNow, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, _) = await app.Client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SendEvents_ValidEvents_ReturnsProcessedEventIdsAndAdherenceScore()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        
        var request = new Request([
            new Request.Event(eventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        var client = await app.CreateClientWithToken();
        
        // Act
        var (response, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
       
        result.ShouldNotBeNull();
        result!.ProcessedEventIds.ShouldContain(eventId);
        result.AdherenceScore.ShouldBeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task SendEvents_DuplicateEvents_OnlyProcessesOnce()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var eventId = Guid.NewGuid();
        var request = new Request([
            new Request.Event(eventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        // Act - send same event twice
        var (_, firstResult) = await client.POSTAsync<Endpoint, Request, Response>(request);
        var (_, secondResult) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        firstResult!.ProcessedEventIds.ShouldContain(eventId);
        secondResult!.ProcessedEventIds.ShouldNotContain(eventId); // duplicate should not be processed
    }

    [Fact]
    public async Task SendEvents_MultipleEventsWithDuplicate_OnlyProcessesNewEvents()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var existingEventId = Guid.NewGuid();
        var newEventId = Guid.NewGuid();

        // First, send an event
        var firstRequest = new Request([
            new Request.Event(existingEventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);
        await client.POSTAsync<Endpoint, Request, Response>(firstRequest);

        // Act - send batch with both existing and new event
        var secondRequest = new Request([
            new Request.Event(existingEventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled)),
            new Request.Event(newEventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);
        var (_, result) = await client.POSTAsync<Endpoint, Request, Response>(secondRequest);

        // Assert
        result!.ProcessedEventIds.ShouldContain(newEventId);
        result.ProcessedEventIds.ShouldNotContain(existingEventId);
    }

    [Fact]
    public async Task SendEvents_EventWithUnownedDeviceId_IsFiltered()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var unownedDeviceId = Guid.NewGuid(); // Device not owned by patient
        var eventId = Guid.NewGuid();
        var request = new Request([
            new Request.Event(eventId, DateTime.Today, unownedDeviceId, nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result!.ProcessedEventIds.ShouldNotContain(eventId);
    }

    [Fact]
    public async Task SendEvents_MixedOwnedAndUnownedDevices_OnlyProcessesOwned()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var ownedEventId = Guid.NewGuid();
        var unownedEventId = Guid.NewGuid();
        var request = new Request([
            new Request.Event(ownedEventId, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled)),
            new Request.Event(unownedEventId, DateTime.Today, Guid.NewGuid(), nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (_, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        result!.ProcessedEventIds.ShouldContain(ownedEventId);
        result.ProcessedEventIds.ShouldNotContain(unownedEventId);
    }

    [Fact]
    public async Task SendEvents_EmptyEventsList_ReturnsOkWithZeroProcessed()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var request = new Request([]);

        // Act
        var (response, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result!.ProcessedEventIds.ShouldBeEmpty();
    }

    [Fact]
    public async Task SendEvents_EmptyEventId_ReturnsBadRequest()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var request = new Request([
            new Request.Event(Guid.Empty, DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, _) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.AsProblemDetailsAsync(TestContext.Current.CancellationToken);
        problemDetails
            .ShouldContainError("'event Id' must not be empty.", "events[0].eventId");
    }

    [Fact]
    public async Task SendEvents_EmptyDeviceId_ReturnsBadRequest()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var request = new Request([
            new Request.Event(Guid.NewGuid(), DateTime.Today, Guid.Empty, nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, _) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.AsProblemDetailsAsync(TestContext.Current.CancellationToken);
        problemDetails
            .ShouldContainError("'device Id' must not be empty.", "events[0].deviceId");
    }

    [Fact]
    public async Task SendEvents_InvalidEventType_ReturnsBadRequest()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var request = new Request([
            new Request.Event(Guid.NewGuid(), DateTime.Today, SeedData.Patient.DeviceIds[0], "InvalidType")
        ]);

        // Act
        var (response, _) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.AsProblemDetailsAsync(TestContext.Current.CancellationToken);

        problemDetails
            .ShouldContainError("Invalid event type", "events[0].eventType");
    }

    [Fact]
    public async Task SendEvents_EmptyEventType_ReturnsBadRequest()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var request = new Request([
            new Request.Event(Guid.NewGuid(), DateTime.Today, SeedData.Patient.DeviceIds[0], "")
        ]);

        // Act
        var (response, _) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.AsProblemDetailsAsync(TestContext.Current.CancellationToken);

        problemDetails
            .ShouldContainError("'event type' must not be empty.", "events[0].eventType");
    }

    [Fact]
    public async Task SendEvents_MultipleValidEvents_AllProcessed()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var eventIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        
        var request = new Request([
            new Request.Event(eventIds[0], DateTime.Today, SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled)),
            new Request.Event(eventIds[1], DateTime.Today.AddHours(-1), SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled)),
            new Request.Event(eventIds[2], DateTime.Today.AddHours(-2), SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result!.ProcessedEventIds.Count().ShouldBe(3);
        foreach (var eventId in eventIds)
        {
            result.ProcessedEventIds.ShouldContain(eventId);
        }
    }

    [Fact]
    public async Task SendEvents_EventsWithOutOfOrderTimestamps_AllProcessed()
    {
        // Arrange
        var client = await app.CreateClientWithToken();
        var pastEventId = Guid.NewGuid();
        var futureEventId = Guid.NewGuid();

        var request = new Request([
            new Request.Event(futureEventId, DateTime.Today.AddDays(1), SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled)),
            new Request.Event(pastEventId, DateTime.Today.AddDays(-10), SeedData.Patient.DeviceIds[0], nameof(EventType.PuffInhaled))
        ]);

        // Act
        var (response, result) = await client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        result!.ProcessedEventIds.ShouldContain(pastEventId);
        result.ProcessedEventIds.ShouldContain(futureEventId);
    }
}
