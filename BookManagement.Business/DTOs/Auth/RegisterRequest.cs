using System.ComponentModel.DataAnnotations;

namespace BookManagement.Business.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
