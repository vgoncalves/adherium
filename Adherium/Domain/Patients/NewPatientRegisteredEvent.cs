namespace Adherium.Domain.Patients;

public record NewPatientRegisteredEvent(Guid PatientId, string Name, string Email);
