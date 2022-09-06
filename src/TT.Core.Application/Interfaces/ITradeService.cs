using TT.Core.Application.Dtos.Inputs;

namespace TT.Core.Application.Interfaces;

public interface ITradeService
{
    Task<CreateTradeDto> GetAll();
    Task Create(CreateTradeDto tradeDto);
}
