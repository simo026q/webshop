using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Api.Auth;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Extensions;
using Webshop.Api.Repositories;

namespace Webshop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [AuthorizeRole(UserRole.Employee, UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders(int page = 1, int size = 50)
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders.Count == 0)
                return NoContent();

            var paginatedOrders = orders.ToPaginated(page, size);

            HttpContext.Response.Headers.Add("Total-Count", orders.Count.ToString());

            var result = orders
                .Select(OrderResponse.FromEntity)
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

    [HttpGet("{id}")]
    [AuthorizeRole(UserRole.Employee, UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);

        if (order == null)
            return NotFound();

        return Ok(OrderResponse.FromEntity(order));
    }

    [AuthorizeRole(UserRole.Customer, UserRole.Employee, UserRole.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest value)
    {
        try
        {
            foreach(var product in value.Products)
            {
                var productVariant = await _productRepository.GetProductVariantByIdAsync(product.ProductVariantId);

                if (productVariant == null)
                    continue;

                if (productVariant.Stock < product.Quantity)
                    return BadRequest("Not enough stock");

                productVariant.Stock -= product.Quantity;
            }

            var item = await _orderRepository.CreateAsync(value.ToEntity());

            return CreatedAtAction(nameof(GetOrder), new { id = item.Id }, OrderResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPatch("{id}/status")]
    [AuthorizeRole(UserRole.Employee, UserRole.Admin)]
    public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(Guid id, [FromBody] OrderStatusRequest value)
    {
        var order = await _orderRepository.GetAsync(id);

        if (order == null)
            return NotFound();

        order.Status = value.Status;

        try
        {
            var item = await _orderRepository.UpdateAsync(order);

            return Ok(OrderResponse.FromEntity(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [AuthorizeRole(UserRole.Customer, UserRole.Employee, UserRole.Admin)]
    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderResponse>> GetOrderByUser(Guid id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Problem("User id is not valid");
        
        var order = await _orderRepository.GetAsync(id);

        if (order == null)
            return NotFound();

        if (order.UserId != userId)
            return Forbid();

        return Ok(OrderResponse.FromEntity(order));
    }

    [AuthorizeRole(UserRole.Customer, UserRole.Employee, UserRole.Admin)]
    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrdersByUser(int page = 1, int size = 50)
    {
        var userId = GetUserId();

        if (userId == null)
            return Problem("User id is not valid");

        try
        {
            var orders = await _orderRepository.GetAllByUserIdAsync(userId.Value);

            if (orders.Count == 0)
                return NoContent();

            var paginatedOrders = orders.ToPaginated(page, size);

            HttpContext.Response.Headers.Add("Total-Count", orders.Count.ToString());

            var result = orders
                .Select(OrderResponse.FromEntity)
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

    private Guid? GetUserId()
    {
        var id = User.FindFirstValue("id");

        if (!Guid.TryParse(id, out var guid))
            return null;

        return guid;
    }
}
