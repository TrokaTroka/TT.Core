using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IResetPasswordRepository
{
    Task<ResetPassword> GetByDocument(string document);
    Task<ResetPassword> GetByHashAndCpf(string hash, string document);
    Task Add(ResetPassword resetPassword);
    Task Update(ResetPassword resetPassword);
    Task Delete(ResetPassword resetPassword);
}
