using FluentValidation;
using TT.Core.Application.Constants;
using TT.Core.Application.Dtos.Inputs;

namespace TT.Core.Application.Dtos.Validators;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(rp => rp.Hash)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(rp => rp.NewPassword)
            .Equal(rp => rp.NewPasswordConfirm).WithMessage("As Senhas devem ser iguais")
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .MinimumLength(8).WithMessage(ErrorMessages.MinMessage).WithName("Senha")
            .Matches(@"[A-Z]+").WithMessage("O campo Senha deve conter ao menos 1 letra maiuscula")
            .Matches(@"[a-z]+").WithMessage("O campo Senha deve conter ao menos 1 letra minuscula")
            .Matches(@"[0-9]+").WithMessage("O campo Senha deve conter ao menos 1 número");

        RuleFor(rp => rp.NewPasswordConfirm)
             .Equal(rp => rp.NewPassword).WithMessage("As Senhas devem ser iguais")
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .MinimumLength(8).WithMessage(ErrorMessages.MinMessage).WithName("Senha")
            .Matches(@"[A-Z]+").WithMessage("O campo Senha deve conter ao menos 1 letra maiuscula")
            .Matches(@"[a-z]+").WithMessage("O campo Senha deve conter ao menos 1 letra minuscula")
            .Matches(@"[0-9]+").WithMessage("O campo Senha deve conter ao menos 1 número");
    }
}
