using Microsoft.Extensions.Configuration;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Integration.Azure;

namespace TT.Core.Application.Services;

public class UserService : IUserService
{        
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogErrorRepository _log;

    private readonly string _blobConnectionString;
    private const string _blobContainerNamer = "avatars";

    public UserService(IUserRepository userRepository,
        IAccountRepository accountRepository,
        ILogErrorRepository log,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _blobConnectionString = configuration.GetConnectionString("BlobStorage");
        _log = log;
    }

    public async Task<Attempt<Failure, UserVM>> UpdateUser(UpdateUserDto userDto)
    {
        var failure = new Failure();
        try
        {
            var user = await _userRepository.GetById(userDto.Id);

            if (await _userRepository.GetUserByEmail(userDto.Email) is not null
                && user.Email != userDto.Email)
            {
                failure.SetMessage("Email já cadastrado");
                return failure;
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
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(BookService), nameof(UpdateUser), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> ActiveAccount(Guid key)
    {
        var failure = new Failure();
        try
        {
            var account = await _accountRepository.GetByKey(key);

            if (account is null)
            {
                failure.SetMessage("Key não existe");
                return failure;
            }

            account.ActiveAccount();

            await _accountRepository.Update(account);

            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(BookService), nameof(UpdateUser), ex.Message));
            return failure;
        }
    }
}
