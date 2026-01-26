namespace Adherium.Api.Features.Authenticate;

public record Request(string Email, string Password);

public record Response(string Token);
