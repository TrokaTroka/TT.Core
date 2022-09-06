using TT.Core.Application.Dtos.Inputs;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface ICategoryService
{
    Task<Attempt<Failure, List<Category>>> GetAllCategories();
    Task<Attempt<Failure, Category>> GetCategoryById(Guid idCategory);
    Task<Attempt<Failure, Guid>> CreateCategory(CreateCategoryDto createCategoryDto);
    Task<Attempt<Failure, bool>> UpdateCategory(UpdateBookDto inputModel);
    Task<Attempt<Failure, bool>> DeleteCategory(Guid idCategory);
}
