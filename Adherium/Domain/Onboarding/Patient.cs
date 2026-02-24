using Adherium.Domain.Auth;

namespace Adherium.Domain.Onboarding;

public class Patient : User
{
    public required DateTime BirthDate { get; set; }
}
