using FluentValidation;
using TT.Core.Application.Constants;

namespace TT.Core.Application.Dtos.Inputs.Validators;

public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage);

        RuleFor(b => b.Title)
           .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Título")
           .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Título")
           .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Título")
           .MaximumLength(100).WithMessage(ErrorMessages.MaxMessage).WithName("Título");

        RuleFor(b => b.Author)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Autor do livro")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Autor do livro")
            .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Autor do livro")
            .MaximumLength(100).WithMessage(ErrorMessages.MaxMessage).WithName("Autor do livro");

        RuleFor(b => b.Isbn)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage)
            .NotNull().WithMessage(ErrorMessages.RequiredMessage)
            .MinimumLength(10).WithMessage(ErrorMessages.MinMessage)
            .MaximumLength(13).WithMessage(ErrorMessages.MaxMessage);

        RuleFor(b => b.Publisher)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Editora")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Editora")
            .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Editora")
            .MaximumLength(50).WithMessage(ErrorMessages.MaxMessage).WithName("Editora");

        RuleFor(b => b.Reason)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Razão de troca")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Razão de troca")
            .MinimumLength(3).WithMessage(ErrorMessages.MinMessage).WithName("Razão de troca")
            .MaximumLength(100).WithMessage(ErrorMessages.MaxMessage).WithName("Razão de troca");

        RuleFor(b => b.BuyPrice)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Preço de Compra")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Preço de Compra");

        RuleFor(b => b.BuyDate)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Data Compra")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Data Compra");

        RuleFor(b => b.Model)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Capa")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Capa");

        RuleFor(b => b.Language)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Lingua do livro")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Lingua do livro");

        RuleFor(b => b.IdCategory)
            .NotEmpty().WithMessage(ErrorMessages.RequiredMessage).WithName("Categoria")
            .NotNull().WithMessage(ErrorMessages.RequiredMessage).WithName("Categoria");
    }       
}
