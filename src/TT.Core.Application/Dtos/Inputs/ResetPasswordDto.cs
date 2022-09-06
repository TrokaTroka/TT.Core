namespace TT.Core.Application.Dtos.Inputs;

public class ResetPasswordDto
{
    public string Hash { get; set; }
    public string Document { get; set; }

    public string NewPassword { get; set; }

    public string NewPasswordConfirm { get; set; }
}
