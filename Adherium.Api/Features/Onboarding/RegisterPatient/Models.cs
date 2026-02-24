namespace Adherium.Api.Features.Onboarding.RegisterPatient;

public record Request(string Name, DateTime BirthDate, string Email, string Password);