using FastEndpoints;
using FluentValidation;

namespace Adherium.Api.Features.Auth.AuthenticateUser;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
