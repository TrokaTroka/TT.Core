using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IAuthenticatedUser _user;
    private readonly IUserRepository _userRepository;
    private readonly ILogErrorRepository _log;

    public RatingService(IRatingRepository ratingRepository,
        IAuthenticatedUser user,
        IUserRepository userRepository,
        ILogErrorRepository log)
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
        _user = user;
        _log = log;
    }
            
    public async Task<Attempt<Failure, Guid>> Create(CreateRatingDto ratingInput)
    {
        var failure = new Failure();
        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            if (user is null)
            {
                failure.SetMessage("Usuário não encontrado");
                return failure;
            }

            if (user.Id.Equals(ratingInput.IdRated))
            {
                failure.SetMessage("Não é possivel auto avaliar");
                return failure;
            }

            if (ExistRating(user.Id, ratingInput.IdRated))
            {
                failure.SetMessage("Usuário já foi avaliado");
                return failure;
            }

            var rating = new Rating(ratingInput.Grade,
                ratingInput.Comment, user.Id, ratingInput.IdRated);

            await _ratingRepository.Create(rating);

            return rating.Id;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(RatingService), nameof(Create), ex.Message));
            return failure;
        }
    }

    private bool ExistRating(Guid userId, Guid idRated)
    {
        return _ratingRepository.SearchAll(c => c.IdRater == userId && c.IdRated == idRated).Result.Any();
    }
}
