namespace TT.Core.Application.Helpers;

public static class DocumentHelper
{
    public static bool IsCpfValido(string cpf)
    {
        while (cpf.Length < 11)
            cpf = "0" + cpf;
        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "");
        if (cpf.Length != 11)
            return false;
        var cpfTemporario = cpf[..9];
        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador1[i];
        var resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else resto = 11 - resto;
        var digito = resto.ToString();
        cpfTemporario += digito;
        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else resto = 11 - resto;
        digito += resto;
        return cpf.EndsWith(digito);
    }
}
