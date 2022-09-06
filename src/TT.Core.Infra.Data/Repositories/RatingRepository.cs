using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class RatingRepository : BaseRepository<Rating>, IRatingRepository
{
    public RatingRepository(TrokaTrokaDbContext context) : base(context)
    { }

}
