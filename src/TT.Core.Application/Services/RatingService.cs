using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class RatingService : BaseService, IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IAuthenticatedUser _user;
    private readonly IUserRepository _userRepository;

    public RatingService(IRatingRepository ratingRepository,
        IAuthenticatedUser user,
        IUserRepository userRepository,
        INotifier notifier) : base(notifier)
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
        _user = user;
    }
            
    public async Task<Guid> Create(CreateRatingDto ratingInput)
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        if (user is null)
        {
            Notify("Usuário não encontrado");
            return Guid.Empty;
        }

        if(user.Id.Equals(ratingInput.IdRated))
        {
            Notify("Não é possivel auto avaliar");
            return Guid.Empty;
        }

        if (ExistRating(user.Id, ratingInput.IdRated))
        {
            Notify("Usuário já foi avaliado");
            return Guid.Empty;
        }

        var rating = new Rating(ratingInput.Grade,
            ratingInput.Comment, user.Id, ratingInput.IdRated);

        await _ratingRepository.Create(rating);
        
        return rating.Id;
    }

    private bool ExistRating(Guid userId, Guid idRated)
    {
        return _ratingRepository.SearchAll(c => c.IdRater == userId && c.IdRated == idRated).Result.Any();
    }
}
