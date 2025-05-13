using ch_16_jwt.Entities.DTOs;
using ch_16_jwt.Entities.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace ch_16_jwt.Abstracts;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(UserDtoForRegister? userDtoForRegister);
    Task<bool> ValidateUserAsync(UserDtoForAuthentication? userDtoForAuthentication);

    Task<TokenDto> CreateTokenAsync(bool populateExpiredAccessToken);
    
    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
}