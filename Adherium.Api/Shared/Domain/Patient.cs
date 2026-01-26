namespace Adherium.Api.Shared.Domain;

/// The user in our case is the patient.  
public record Patient(
    Guid Id,
    string Name,
    string Email,
    string Password,
    List<Guid> DeviceIds);