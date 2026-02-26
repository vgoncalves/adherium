using Adherium.Infra;
using Microsoft.EntityFrameworkCore;

namespace Adherium.Domain.Auth;

public class AuthorizationService(AppDb appDb)
{
    public Task<bool> IsDeviceAssignedToPatient(Guid deviceId, Guid patientId)
    {
        return appDb.Patients.AnyAsync(x => x.Id == patientId && x.DeviceId == deviceId);
    }
}