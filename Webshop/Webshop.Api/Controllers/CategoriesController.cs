using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.Auth;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;

namespace Webshop.Api.Controllers;

[AuthorizeRole(UserRole.Employee, UserRole.Admin)]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategoriesWithProduct()
    {
        var categories = await _categoryRepository.GetAllWithProductsAsync();

        var result = categories.Select(CategoryResponse.FromEntity).ToList();

        return Ok(result);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var result = categories.Select(CategoryResponse.FromEntity).ToList();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryRequest categoryRequest)
    {
        var category = categoryRequest.ToEntity();

        try
        {
            var createdCategory = await _categoryRepository.CreateAsync(category);

            return CreatedAtAction(null, CategoryResponse.FromEntity(createdCategory));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryResponse>> DeleteCategory(string id)
    {
        try
        {
            var deletedCategory = await _categoryRepository.DeleteAsync(id);

            if (deletedCategory == null)
                return NotFound();

            return Ok(CategoryResponse.FromEntity(deletedCategory));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
