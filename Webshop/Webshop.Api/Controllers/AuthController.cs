using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.Auth;
using Webshop.Api.Services;

namespace Webshop.Api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenManagerService _tokenManager;

    public AuthController(ITokenManagerService tokenManager)
    {
        _tokenManager = tokenManager;
    }

    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest authRequest)
    {
        var token = await _tokenManager.GenerateTokenAsync(authRequest);

        return Ok(new AuthResponse(token));
    }
}
