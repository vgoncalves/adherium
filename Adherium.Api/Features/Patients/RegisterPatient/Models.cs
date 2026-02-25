namespace Adherium.Api.Features.Patients.RegisterPatient;

public record Request(string Name, DateTime BirthDate, string Email, string Password);