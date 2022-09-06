using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogErrorRepository _log;

    public CategoryService(ICategoryRepository categoryRepository, 
        ILogErrorRepository log)
    {
        _categoryRepository = categoryRepository;
        _log = log;
    }

    public async Task<Attempt<Failure, List<Category>>> GetAllCategories()
    {
        var failure = new Failure();
        try
        {
            var categoriesVM = await _categoryRepository.GetAll();
            return categoriesVM.OrderBy(c => c.Name).ToList();
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(CategoryService), nameof(GetAllCategories), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, Category>> GetCategoryById(Guid idCategory)
    {
        var failure = new Failure();
        try
        {
            var categoryVM = await _categoryRepository.GetById(idCategory);
            return categoryVM;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(CategoryService), nameof(GetCategoryById), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, Guid>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var failure = new Failure();
        try
        {
            var category = new Category(createCategoryDto.Name);
            await _categoryRepository.Create(category);
            return category.Id;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(CategoryService), nameof(CreateCategory), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> UpdateCategory(UpdateBookDto inputModel)
    {
        var failure = new Failure();
        try
        {

            await _categoryRepository.Update(new Category(inputModel.IdUser.ToString()));
            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(CategoryService), nameof(UpdateCategory), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> DeleteCategory(Guid idBook)
    {
        var failure = new Failure();
        try
        {
            _categoryRepository.Delete(new Category(idBook.ToString()));
            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(CategoryService), nameof(DeleteCategory), ex.Message));
            return failure;
        }
    }
}
