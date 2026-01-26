namespace Adherium.Api.Shared.Domain;

public interface IPatientRepository
{
    Patient? GetPatient(string email, string password);
}