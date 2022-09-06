using TT.Core.Application.Dtos.Inputs;

namespace TT.Core.Application.Interfaces;

public interface IRatingService
{
    Task<Guid> Create(CreateRatingDto ratingDto);
}
