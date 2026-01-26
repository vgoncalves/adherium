using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace Adherium.Api.Shared;

public static class ClaimsPrincipalExtensions
{
    public static Patient AsPatient(this ClaimsPrincipal principal)
    {
        var id = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var name = principal.FindFirstValue(ClaimTypes.Name)!;
        var email = principal.FindFirstValue(ClaimTypes.Email)!;
        
        var deviceIds = JsonSerializer.Deserialize<List<Guid>>(principal.FindFirstValue(nameof(Patient.DeviceIds))!)!;

        return new Patient(id, name, email, Password: string.Empty, deviceIds);
    }
}