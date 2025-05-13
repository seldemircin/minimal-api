using ch_15_identity.Entities.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace ch_15_identity.Abstracts;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(UserDtoForRegister? userDtoForRegister);
}