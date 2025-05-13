using Microsoft.AspNetCore.Identity;

namespace ch_15_identity.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}