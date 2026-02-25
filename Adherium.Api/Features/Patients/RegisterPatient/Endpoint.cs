using Adherium.Domain.Auth;
using Adherium.Domain.Patients;
using Adherium.Infra;
using FastEndpoints;

namespace Adherium.Api.Features.Patients.RegisterPatient;

public class Endpoint(AppDb db, EventPublisher eventPublisher) : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/onboarding/patients");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var newPatient = new Patient
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            BirthDate = request.BirthDate,
            Email = request.Email,
            PasswordHash = request.Password,
            UserRole = UserRole.Patient,
        };

        db.Patients.Add(newPatient);
        await db.SaveChangesAsync(ct);

        await eventPublisher.PublishEvent(new NewPatientRegisteredEvent(newPatient.Id, newPatient.Name, newPatient.Email), ct);
        
        await Send.OkAsync(cancellation: ct);
    }
}