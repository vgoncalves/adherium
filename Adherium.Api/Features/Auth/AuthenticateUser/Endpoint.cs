using System.Security.Claims;
using Adherium.Infra;
using FastEndpoints;
using FastEndpoints.Security;

namespace Adherium.Api.Features.Auth.AuthenticateUser;

public class Endpoint(AppDb db) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var user = db.Users.FirstOrDefault(x => x.PasswordHash == request.Password && x.Email == request.Email); // Hash/salt password was omitted for simplicity

        if (user is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = Config["Jwt:SigningKey"]!;
            o.ExpireAt = DateTime.UtcNow.AddHours(1);
            o.User.Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            o.User.Claims.Add(new Claim(ClaimTypes.Name, user.Name));
            o.User.Claims.Add(new Claim(ClaimTypes.Email, user.Email));
            o.User.Claims.Add(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
        });

        await Send.OkAsync(new Response(token), ct);
    }
}