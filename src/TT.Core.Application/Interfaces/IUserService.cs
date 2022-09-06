using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface IUserService
{
    Task<Attempt<Failure, UserVM>> UpdateUser(UpdateUserDto userDto);
    Task<Attempt<Failure, bool>> ActiveAccount(Guid key);
}
