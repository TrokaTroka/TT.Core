using FluentValidation;
using TT.Core.Application.Constants;
using TT.Core.Application.Helpers;

namespace TT.Core.Application.Dtos.Inputs.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress().WithMessage(ErrorMessages.InvalidMessage)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Senha")
            .MinimumLength(8).WithMessage(ErrorMessages.MinMessage).WithName("Senha")
            .Matches(@"[A-Z]+").WithMessage("O campo Senha deve conter ao menos 1 letra maiuscula")
            .Matches(@"[a-z]+").WithMessage("O campo Senha deve conter ao menos 1 letra minuscula")
            .Matches(@"[0-9]+").WithMessage("O campo Senha deve conter ao menos 1 número");
        /*.Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");*/

        RuleFor(c => c.Document)
            .Must(c => DocumentHelper.IsCpfValido(c) == true).WithMessage(ErrorMessages.InvalidMessage).WithName("Cpf")
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Cpf")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Cpf");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Nome")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Nome")
            .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Nome")
            .MaximumLength(100).WithMessage(ErrorMessages.MaxMessage).WithName("Nome");
    }       
}
