using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Interfaces.Helpers;
using TT.Core.Application.Notifications;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class TradeService : BaseService, ITradeService
{
    private readonly string ServiceName = "Trade";

    private readonly ITradeRepository _tradRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticatedUser _user;
    private readonly ILogErrorRepository _log;
    private readonly IEmailHelper _mailHelper;

    public TradeService(ITradeRepository tradRepository,
        IBookRepository bookRepository,
        IUserRepository userRepository,
        IAuthenticatedUser user,
        ILogErrorRepository log,
        IEmailHelper mailHelper,
        INotifier notifier) : base(notifier)
    {
        _tradRepository = tradRepository;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _log = log;
        _user = user;
        _mailHelper = mailHelper;
    }

    public async Task Create(CreateTradeDto tradeDto)
    {
        try
        {
            var email = _user.GetEmailUserLogged();

            var bookRecived = await _bookRepository.GetBookById(tradeDto.IdBookReceived);

            var bookGived = await _bookRepository.GetBookById(tradeDto.IdBookGived);
            var userGived = await _userRepository.GetUserByEmail(email);

            var trade = new Trade(tradeDto.IdBookReceived, bookRecived.IdUser, tradeDto.IdBookGived, userGived.Id);

            var tags = new Dictionary<string, string>
            {
                { "BookGivedTitle", bookGived.Title },
                { "BookGivedUserName", bookGived.Owner.Name }
            };

            var template = new Template(TemplateType.WantTrade, tags);

            var notification = new NotificationRequest($"{userGived.Name} deseja fazer uma troca por livro {bookRecived.Title}", bookRecived.Owner.Email, template);

            var attempt = await _mailHelper.Send(notification);

            if (!attempt.Succeeded)
                return ;

            await _tradRepository.Create(trade);
        }
        catch (Exception ex)
        {
            var log = new LogError(ServiceName, nameof(Create), ex.Message);
            await _log.Create(log);
            return ;
        }            
    }

    public async Task<CreateTradeDto> GetAll()
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        var trades = await _tradRepository.SearchAll(t => t.IdUserGived == user.Id);

        return new CreateTradeDto();
    }
}
