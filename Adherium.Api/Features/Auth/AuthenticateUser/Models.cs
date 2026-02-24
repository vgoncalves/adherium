namespace Adherium.Api.Features.Auth.AuthenticateUser;

public record Request(string Email, string Password);

public record Response(string Token);
