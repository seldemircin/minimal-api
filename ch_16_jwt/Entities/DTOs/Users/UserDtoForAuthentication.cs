using System.ComponentModel.DataAnnotations;

namespace ch_16_jwt.Entities.DTOs.Users;

public record UserDtoForAuthentication
{
    [Required]
    public string UserName { get; init; }
    [Required]
    public string Password { get; init; }
}