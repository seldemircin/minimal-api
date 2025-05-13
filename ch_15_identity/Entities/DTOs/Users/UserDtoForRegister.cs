using System.ComponentModel.DataAnnotations;

namespace ch_15_identity.Entities.DTOs.Users;

public record UserDtoForRegister()
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; init; }
    public string? Email { get; init; }
    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
    public string? PhoneNumber { get; init; }
    public ICollection<string>? Roles { get; init; }
    
}