using Security.Dtos;
using Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service) {
        _service = service;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Authenticate(AuthRequestDto auth)
    {
        try
        {
            var token = _service.GenerateToken(auth);
            return Ok(token);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(UserDto user)
    {
        try
        {
            var response = await _service.CreateUser(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValideToken()
    {
        return Ok();
    }
}
