using Microsoft.Extensions.Configuration;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Integration.Azure;

namespace TT.Core.Application.Services;

public class UserService : BaseService, IUserService
{        
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    
    private readonly string _blobConnectionString;
    private const string _blobContainerNamer = "avatars";

    public UserService(IUserRepository userRepository,
        IAccountRepository accountRepository,
        IConfiguration configuration,
        INotifier notifier) : base(notifier)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _blobConnectionString = configuration.GetConnectionString("BlobStorage");
    }

    public async Task<UserVM> UpdateUser(UpdateUserDto userDto)
    {
        var user = await _userRepository.GetById(userDto.Id);

        if (await _userRepository.GetUserByEmail(userDto.Email) is not null 
            && user.Email != userDto.Email)
        {
            Notify("Email já cadastrado");
            return null;
        }

        if (userDto.Avatar is not null)
        {
            var name = user.Id.ToString();

            var uri = await BlobStorage.Upload(_blobConnectionString, _blobContainerNamer, userDto.Avatar, name);

            user.UpdateAvatar(name, uri);
        }
        
        user.UpdateUser(userDto.Name, userDto.Document, userDto.Email);

        await _userRepository.Update(user);

        return new UserVM()
        {
            Id = user.Id,
            Name = user.Name,
            Document = user.Document,
            Email = user.Email,
            AvatarPath = user.AvatarPath
        };
    }

    public async Task ActiveAccount(Guid key)
    {
        var account = await _accountRepository.GetByKey(key);

        if (account is null)
            Notify("Key não existe");

        if (!OperationValid())
            return ;

        account.ActiveAccount();

        await _accountRepository.Update(account);
    }
}
