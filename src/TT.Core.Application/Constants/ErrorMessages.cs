namespace TT.Core.Application.Constants;

public static class ErrorMessages
{
    public const string RequiredMessage = "O campo {PropertyName} é obrigatório";
    public const string InvalidMessage = "O campo {PropertyName} está inválido";
    public const string MinMessage = "O campo {PropertyName} deve conter ao menos {MinLength} caracteres";
    public const string MaxMessage = "O campo {PropertyName} deve conter no máximo {MaxLength} caracteres";
}
