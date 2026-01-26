using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Security;

namespace Adherium.Api.Features.Authenticate;

public class Endpoint(IPatientRepository patientRepository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var patient = patientRepository.GetPatient(request.Email, request.Password);

        if (patient is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = Config["Jwt:SigningKey"]!;
            o.ExpireAt = DateTime.UtcNow.AddHours(1);
            o.User.Claims.Add(new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()));
            o.User.Claims.Add(new Claim(ClaimTypes.Name, patient.Name));
            o.User.Claims.Add(new Claim(ClaimTypes.Email, patient.Email));
            o.User.Claims.Add(new Claim(nameof(Patient.DeviceIds), JsonSerializer.Serialize(patient.DeviceIds)));
        });

        await Send.OkAsync(new Response(token), ct);
    }
}