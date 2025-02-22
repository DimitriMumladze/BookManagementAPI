using BookManagement.Business.DTOs.Auth;
using BookManagement.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest command)
    {
        try
        {
            var result = await _identityService.Login(command);
            if (result.Succeeded)
                return Ok(new { Token = result.Token, Message = "Login successful" });
            return Unauthorized(new { Message = "Invalid username or password" });
        }
        catch (Exception ex) {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest command)
    {
        try
        {
            var result = await _identityService.Register(command);
            if (result.Succeeded)
                return Ok(new { Message = "User registered successfully" });
            return BadRequest(new { Errors = result.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
    }
}
