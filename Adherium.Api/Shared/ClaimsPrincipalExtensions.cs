using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Adherium.Domain.Auth;

namespace Adherium.Api.Shared;

public static class ClaimsPrincipalExtensions
{
    public static User Map(this ClaimsPrincipal principal)
    {
        var id = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var name = principal.FindFirstValue(ClaimTypes.Name)!;
        var email = principal.FindFirstValue(ClaimTypes.Email)!;
        var role = Enum.Parse<UserRole>(principal.FindFirstValue(ClaimTypes.Role)!);

        return new User()
        {
            Id = id,
            Name = name,
            Email = email,
            PasswordHash = "",
            UserRole = role
        };
    }
}