using FluentValidation;
using TT.Core.Application.Constants;
using TT.Core.Application.Helpers;

namespace TT.Core.Application.Dtos.Inputs.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(u => u.Id)
           .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
           .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(u => u.Email)
            .EmailAddress().WithMessage(ErrorMessages.InvalidMessage)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(u => u.Document)
            .Must(d => DocumentHelper.IsCpfValido(d) == true).WithMessage(ErrorMessages.InvalidMessage).WithName("Cpf")
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Cpf")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Cpf");

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Nome")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Nome")
            .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Nome")
            .MaximumLength(100).WithMessage(ErrorMessages.MaxMessage).WithName("Nome");
    }       
}
