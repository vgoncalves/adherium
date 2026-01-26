using Adherium.Api.Shared.Domain;

namespace Adherium.Api.Shared.Infra;

public class SeedData
{
    public static Patient Patient = new(new Guid("e7231918-4d22-4920-b661-d82046e8f125"),
        "Walter White",
        "walter@white.com",
        "secret",
        [new Guid("5affadcc-c79c-4a4e-877e-531169a3de36")]);
}