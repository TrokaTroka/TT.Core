using System.Text.RegularExpressions;
using TT.Core.Domain.Constants;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces.Helpers;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Domain.Entities;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Helpers;

namespace TT.Core.Application.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IResetPasswordRepository _resetPasswordRepository;
    private readonly ILogErrorRepository _log;
    private readonly ITokenHelper _tokenHelper;
    private readonly IEmailHelper _mailHelper;

    private readonly string ServiceName = "AuthenticateService";

    public AuthenticateService(IUserRepository userRepository,
        IAccountRepository accountRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ILogErrorRepository log,
        IResetPasswordRepository resetPasswordRepository,
        ITokenHelper tokenHelper,
        IEmailHelper mailHelper)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _log = log;
        _resetPasswordRepository = resetPasswordRepository;
        _tokenHelper = tokenHelper;
        _mailHelper = mailHelper;
    }

    public async Task<Attempt<Failure, TokenVM>> AuthenticateUser(LoginDto inputDto)
    {
        var failure = new Failure();

        try
        {
            var user = await _userRepository.UserAuth(inputDto.Email, EncryptHelper.ComputeSha256Hash(inputDto.Password));

            if (user is null)
            {
                failure.SetMessage("Email ou senha incorretos");
                return failure;
            }

            if (!user.Account.IsActive)
            {
                failure.SetMessage("Conta ainda não esta ativa");
                return failure;
            }

            var refreshToken = await GenerateRefreshToken(inputDto.Email);

            var refreshTokenVM = new RefreshTokenVM(refreshToken.Username, refreshToken.Token, refreshToken.ExpirationDate);
            var userVM = new UserVM(user.Id, user.Name, user.Document, user.Email, user.AvatarPath);

            return await GenerateToken(refreshTokenVM, userVM);
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(AuthenticateUser), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, UserVM>> CreateUser(CreateUserDto inputDto)
    {
        var failure = new Failure();

        try
        {
            if (EmailExists(inputDto.Email))
            {
                failure.SetMessage("Email já cadastrado");
                return failure;
            }

            if (CpfExists(inputDto.Document))
            {
                failure.SetMessage("Cpf já cadastrado");
                return failure;
            }

            var user = new User(inputDto.Email, EncryptHelper.ComputeSha256Hash(inputDto.Password), inputDto.Name, inputDto.Document);

            var key = Guid.NewGuid();
            var account = new Account(key, user.Id);

            var url = $"https://trokatroka.online/ativar-conta?key={key}";

            var tags = new Dictionary<string, string>
            {
                { "Name", user.Name },
                { "Url", url }
            };

            var template = new Template(TemplateType.ActiveAccount, tags);

            var notification = new NotificationRequest($"Seja bem vindo(a), {user.Name}! Ative sua conta.", user.Email, template);

            var attempt = await _mailHelper.Send(notification);

            if (!attempt.Succeeded)
                return failure;

            await _userRepository.Create(user);
            await _accountRepository.Create(account);

            return new UserVM(user.Id, user.Name, user.Document, user.Email, user.AvatarPath);
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(CreateUser), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, TokenVM>> GenerateToken(RefreshTokenVM inputDto, UserVM? inputUserDto = null)
    {
        var failure = new Failure();

        try
        {
            if (inputUserDto is null)
            {
                var user = await _userRepository.GetUserByEmail(inputDto.Username);
                if (user is null)
                {
                    failure.SetMessage("Usuário não encontrado");
                    return failure;
                }

                inputUserDto = new UserVM(user.Id, user.Name, user.Document, user.Email, user.AvatarPath);
            }

            var token = _tokenHelper.GenerateJwtToken(inputDto.Username);

            var userDto = new UserVM(inputUserDto.Id, inputUserDto.Name, inputUserDto.Document, inputUserDto.Email, inputUserDto.AvatarPath);

            return new TokenVM(userDto, token, inputDto.Token, inputDto.ExpirationDate);
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(GenerateToken), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, RefreshTokenVM>> GetRefreshToken(Guid refreshToken)
    {
        var failure = new Failure();

        try
        {
            var token = await _refreshTokenRepository.SearchOne(rt => rt.Token == refreshToken);

            if (token is null || token.ExpirationDate.ToLocalTime() < DateTime.Now)
            {
                failure.SetMessage("Refresh token expirado");
                return failure;
            }

            return new RefreshTokenVM(token.Username, token.Token, token.ExpirationDate);
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(GetRefreshToken), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> SendResetPasswordLink(SendLinkResetPasswordDto inputDto)
    {
        var failure = new Failure();

        try
        {
            var user = await _userRepository.GetUserByEmail(inputDto.Email);

            if (user is null)
            {
                failure.SetMessage("Email não cadastrado");
                return failure;
            }

            var codigo = new Random().Next(000000, 999999);
            var exp = DateTime.UtcNow.AddHours(24);

            var tags = new Dictionary<string, string>
        {
            { "Name", user.Name },
            { "Codigo", codigo.ToString() }
        };

            var template = new Template(TemplateType.ResetPassword, tags);

            var notification = new NotificationRequest($"{user.Name.Split(' ')[0]}, aqui está seu código", user.Email, template);

            var attempt = await _mailHelper.Send(notification);

            if (!attempt.Succeeded)
                return failure;

            var resetPassword = await _resetPasswordRepository.GetByDocument(user.Document);

            if (resetPassword is null)
            {
                resetPassword = new ResetPassword(codigo.ToString(), user.Document, exp);

                await _resetPasswordRepository.Add(resetPassword);

                return true;
            }

            await _resetPasswordRepository.Delete(resetPassword);

            resetPassword.UpdateReset(codigo.ToString(), exp);

            await _resetPasswordRepository.Add(resetPassword);

            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(SendResetPasswordLink), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> ResetPassword(ResetPasswordDto inputDto)
    {
        var failure = new Failure();

        try
        {
            var cpf = UsmaskCpf(inputDto.Document);
            var resetPassword = await _resetPasswordRepository.GetByHashAndCpf(inputDto.Hash, cpf);

            if (resetPassword is null)
            {
                failure.SetMessage("O Código ou Cpf incorreto");
                return failure;
            }

            if (resetPassword.Expiration < DateTime.UtcNow)
            {
                failure.SetMessage("O Código está expirado");
                return failure;
            }

            if (!resetPassword.IsActive)
            {
                failure.SetMessage("O Código já foi utilizado");
                return failure;
            }

            var user = await _userRepository.SearchOne(u => u.Document == cpf);

            if (user is null)
            {
                failure.SetMessage("Usuário não encontrado");
                return failure;
            }

            user.ChangePassword(EncryptHelper.ComputeSha256Hash(inputDto.NewPassword));
            resetPassword.DesativeHash();

            await _resetPasswordRepository.Update(resetPassword);
            await _userRepository.Update(user);


            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(ResetPassword), ex.Message));

            return failure;
        }
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

                //var uri = await BlobStorage.Upload(_blobConnectionString, _blobContainerNamer, userDto.Avatar, name);

                user.UpdateAvatar(name, "uri");
            }

            user.UpdateUser(userDto.Name, userDto.Document, userDto.Email);

            await _userRepository.Update(user);

            var userVM = new UserVM()
            {
                Id = user.Id,
                Name = user.Name,
                Document = user.Document,
                Email = user.Email,
                AvatarPath = user.AvatarPath
            };

            return userVM;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(ServiceName, nameof(UpdateUser), ex.Message));

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

            await _log.Create(new LogError(ServiceName, nameof(ActiveAccount), ex.Message));

            return failure;
        }
    }
    private async Task<RefreshToken> GenerateRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(AppSettings.Instance.RefreshTokenExpiration)
        };

        var tokens = await _refreshTokenRepository.SearchAll(rt => rt.Username == email);

        _refreshTokenRepository.DeleteMany(tokens);
        await _refreshTokenRepository.Create(refreshToken);

        return refreshToken;
    }

    private bool EmailExists(string email)
    {
        return _userRepository.GetUserByEmail(email).Result is not null;
    }

    private bool CpfExists(string cpf)
    {
        return _userRepository.SearchAll(u => u.Document == UsmaskCpf(cpf)).Result.Any();
    }

    private static string UsmaskCpf(string cpf)
    {
        return Regex.Replace(cpf, "[^0-9]", "");
    }
}
