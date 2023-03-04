using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.Auth;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Extensions;
using Webshop.Api.Repositories;

namespace Webshop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthorizeRole(UserRole.Admin)]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<UserResponse>>> GetAllUsers(int page = 1, int size = 50)
    {
        try
        {
            var users = await _userRepository.GetAllAsync();

            if (users.Count == 0)
                return NoContent();

            var paginatedUsers = users.ToPaginated(page, size);

            HttpContext.Response.Headers.Add("Total-Count", users.Count.ToString());

            var result = paginatedUsers
                .Select(UserResponse.FromEntity)
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {   
        var user = await _userRepository.GetAsync(id);

        if (user == null)
            return NotFound();

        return Ok(UserResponse.FromEntity(user));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserCreateRequest userRequest)
    {
        var user = userRequest.ToEntity();

        try
        {
            var createdUser = await _userRepository.CreateAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UserResponse.FromEntity(user));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UserUpdateRequest userRequest)
    {
        if (userRequest.Id != id)
            return BadRequest();

        var currentUser = await _userRepository.GetAsync(id);

        if (currentUser == null)
            return NotFound();

        try
        {
            var updatedUser = await _userRepository.UpdateAsync(userRequest.ToEntity(currentUser));

            return Ok(UserResponse.FromEntity(updatedUser));
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
    public async Task<ActionResult<UserResponse>> DeleteUser(Guid id)
    {
        var currUser = await _userRepository.GetAsync(id);

        try
        {
            if (currUser?.Role == UserRole.Admin)
                return Forbid();

            var user = await _userRepository.DeleteAsync(id);

            if (user == null)
                return NotFound();

            return Ok(UserResponse.FromEntity(user));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
