using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface IAuthenticateService
{
    Task<Attempt<Failure, TokenVM>> AuthenticateUser(LoginDto inputDto);
    Task<Attempt<Failure, UserVM>> CreateUser(CreateUserDto inputDto);
    Task<Attempt<Failure, TokenVM>> GenerateToken(RefreshTokenVM inputDto, UserVM? user = null);
    Task<Attempt<Failure, RefreshTokenVM>> GetRefreshToken(Guid refreshToken);
    Task<Attempt<Failure, UserVM>> UpdateUser(UpdateUserDto userDto);
    Task<Attempt<Failure, bool>> SendResetPasswordLink(SendLinkResetPasswordDto inputDto);
    Task<Attempt<Failure, bool>> ResetPassword(ResetPasswordDto inputDto);
    Task<Attempt<Failure, bool>> ActiveAccount(Guid key);
}
