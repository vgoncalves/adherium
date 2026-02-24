namespace Adherium.Domain.Onboarding.Events;

public record NewPatientRegisteredEvent(Guid PatientId, string Name, string Email);
