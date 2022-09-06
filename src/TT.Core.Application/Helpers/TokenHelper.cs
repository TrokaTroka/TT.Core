using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TT.Core.Domain.Constants;
using TT.Core.Application.Interfaces.Helpers;

namespace TT.Core.Application.Helpers;

public class TokenHelper : ITokenHelper
{
    public string GenerateJwtToken(string email)
    {
        var key = AppSettings.Instance.SecretKey;
        var issuer = AppSettings.Instance.Issuer;
        var audience = AppSettings.Instance.Audience;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.CreateToken(token);

        return tokenHandler.WriteToken(stringToken);
    }
}
