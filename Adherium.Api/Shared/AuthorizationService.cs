using Adherium.Infra;
using Microsoft.EntityFrameworkCore;

namespace Adherium.Domain.Auth.Services;

public class AuthorizationService(AppDb appDb)
{
    public Task<bool> IsTreatmentOwnedByUser(Guid requestTreatmentId, Guid userId)
    {
        return appDb.Treatments.AnyAsync(t => t.Id == requestTreatmentId && t.PatientId == userId);
    }
}