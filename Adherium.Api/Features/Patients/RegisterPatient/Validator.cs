using FastEndpoints;
using FluentValidation;

namespace Adherium.Api.Features.Patients.RegisterPatient;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}