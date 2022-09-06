using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.ViewModels;

namespace TT.Core.Application.Interfaces;

public interface IUserService
{
    Task<UserVM> UpdateUser(UpdateUserDto userDto);
    Task ActiveAccount(Guid key);
}
