using FluentValidation;
using TT.Core.Application.Constants;

namespace TT.Core.Application.Dtos.Inputs.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.Email)
            .EmailAddress().WithMessage(ErrorMessages.InvalidMessage)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(l => l.Password)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .MinimumLength(8).WithMessage(ErrorMessages.MinMessage).WithName("Senha")
            .Matches(@"[A-Z]+").WithMessage("O campo Senha deve conter ao menos 1 letra maiuscula")
            .Matches(@"[a-z]+").WithMessage("O campo Senha deve conter ao menos 1 letra minuscula")
            .Matches(@"[0-9]+").WithMessage("O campo Senha deve conter ao menos 1 número");
        /*.Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");*/
    }
}
