using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.Auth;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Extensions;
using Webshop.Api.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webshop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(UserRole.Employee, UserRole.Admin)]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [AllowAnonymous]
    [HttpGet("latest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductResponse>>> GetLatestProducts(int count = 3)
    {
        var products = await _productRepository.GetTopActiveAsync(count);

        var result = products.Select(ProductResponse.FromEntity).ToList();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ProductResponse>>> GetAllProducts(string? category = null, string? search = null, int page = 1, int size = 50)
    {
        try
        {
            var products = await _productRepository.GetByCategoryAndSearchAsync(search, category);

            if (products.Count == 0)
                return NoContent();
            
            var paginatedProducts = products.ToPaginated(page, size);

            HttpContext.Response.Headers.Add("Total-Count", products.Count.ToString());

            var result = paginatedProducts
                .Select(ProductResponse.FromEntity)
                .ToList();

            return Ok(result);
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Page is out of range");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProduct(Guid id)
    {
        var product = await _productRepository.GetAsync(id);

        if (product == null)
            return NotFound();

        var result = ProductResponse.FromEntity(product);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductRequest value)
    {
        try
        {
            var item = await _productRepository.CreateAsync(value.ToEntity());

            return CreatedAtAction(nameof(GetProduct), new { id = item.Id }, ProductResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(Guid id, [FromBody] ProductRequest value)
    {
        if (id != value.Id)
            return BadRequest();

        try
        {
            var item = await _productRepository.UpdateAsync(value.ToEntity());

            return Ok(ProductResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPatch("{id}/disable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponse>> DisableProduct(Guid id)
    {
        try
        {
            var item = await _productRepository.DisableAsync(id);

            if (item == null)
                return NotFound();

            return Ok(ProductResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPatch("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponse>> ActivateProduct(Guid id)
    {
        try
        {
            var item = await _productRepository.ActivateAsync(id);

            if (item == null)
                return NotFound();

            return Ok(ProductResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
