using TT.Core.Application.Dtos.Inputs;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface ITradeService
{
    Task<Attempt<Failure, CreateTradeDto>> GetAll();
    Task<Attempt<Failure, bool>> Create(CreateTradeDto tradeDto);
}
