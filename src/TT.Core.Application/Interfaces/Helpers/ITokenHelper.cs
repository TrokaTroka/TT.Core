namespace TT.Core.Application.Interfaces.Helpers;

public interface ITokenHelper
{        
    string GenerateJwtToken(string email);
}
