using TT.Core.Application.Dtos.Inputs;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface IRatingService
{
    Task<Attempt<Failure, Guid>> Create(CreateRatingDto ratingDto);
}
