using Azure.Core;
using BookManagement.Business.DTOs.Auth;
using BookManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagement.Business.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<LoginDto> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
            throw new Exception("not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new Exception("Invalid email or password.");


        var userRoles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user);

        return new LoginDto
        {
            Succeeded = true,
            Token = token,
        };
    }

    public async Task<RegisterDto> Register(RegisterRequest registerRequest)
    {
        var user = new User()
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
        {
            return new RegisterDto
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new RegisterDto
        {
            Succeeded = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description)
        };
    }

    private string GenerateJwtToken(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var claims = new List<Claim>
        {
            new Claim("id", user.Id),
            new Claim("user_name", user.UserName!),
            new Claim("email", user.Email!),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDate"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
