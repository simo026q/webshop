using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Api.Auth;
using Webshop.Api.Dtos;
using Webshop.Api.Entities;
using Webshop.Api.Repositories;

namespace Webshop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(UserRole.Customer, UserRole.Employee, UserRole.Admin)]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> GetUser()
    {
        var userId = GetUserId();

        if (userId == null)
            return Problem("User id is not valid");

        var user = await _userRepository.GetAsync((Guid)userId);

        if (user == null)
            return NotFound();

        return Ok(UserResponse.FromEntity(user));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> UpdateUser([FromBody] UserUpdateRequest userRequest)
    {
        var userId = GetUserId();

        if (userRequest.Id != userId)
            return Forbid();

        var user = await _userRepository.GetAsync((Guid)userId);

        if (user == null)
            return NotFound();

        var newUser = userRequest.ToEntity(user);

        try
        {
            var updatedUser = await _userRepository.UpdateAsync(newUser);

            return Ok(UserResponse.FromEntity(updatedUser));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> DeleteUser()
    {
        var userId = GetUserId();

        if (userId == null)
            return Problem("User id is not valid");

        try
        {
            var deletedUser = await _userRepository.DeleteAsync((Guid)userId);

            if (deletedUser == null)
                return NotFound();

            return Ok(UserResponse.FromEntity(deletedUser));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var userId = GetUserId();

        if (userId == null)
            return Problem("User id is not valid");

        var user = await _userRepository.GetAsync((Guid)userId);

        if (user == null)
            return NotFound();

        if (!user.CheckPassword(changePasswordRequest.Password))
            return Forbid();

        user.UpdatePassword(changePasswordRequest.NewPassword);

        try
        {
            await _userRepository.UpdateAsync(user);

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> RegisterCustomer([FromBody] RegisterCustomerRequest registerUser)
    {
        var user = registerUser.ToEntity();

        try
        {
            var createdUser = await _userRepository.CreateAsync(user);
            
            return CreatedAtAction(nameof(GetUser), UserResponse.FromEntity(createdUser));
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
