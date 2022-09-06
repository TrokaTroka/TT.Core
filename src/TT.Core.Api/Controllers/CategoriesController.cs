using Microsoft.AspNetCore.Mvc;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Domain.Entities;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : MainController
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoriesController(ICategoryRepository categoryRepository,
        INotifier notifier) : base(notifier)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categoriesVM = await _categoryRepository.GetAll();

        if (categoriesVM == null)
            return NotFound("");
        

        return Ok(categoriesVM.OrderBy(c => c.Name));
    }

    [HttpGet]
    [Route("{idCategory}")]
    public async Task<IActionResult> GetCategoryById(Guid idCategory)
    {
        var categoryVM = await _categoryRepository.GetById(idCategory);

        if (categoryVM == null)
            return BadRequest();

        return Ok(categoryVM);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);            

        await _categoryRepository.Create(new Category(createCategoryDto.Name));
        
        return Created(nameof(GetCategoryById), createCategoryDto);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateBookDto inputModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _categoryRepository.Update(new Category(inputModel.IdUser.ToString()));

        return NoContent();
    }

    [HttpDelete]
    [Route("{idCategory}")]
    public IActionResult DeleteCategory(Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _categoryRepository.Delete(new Category(idBook.ToString()));

        return NoContent();
    }
}
