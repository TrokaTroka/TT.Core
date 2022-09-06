using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces.Helpers;

public interface IEmailHelper
{
    Task<Attempt<Failure, bool>> Send(NotificationRequest notification);
}
