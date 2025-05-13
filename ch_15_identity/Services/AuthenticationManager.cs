using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ch_15_identity.Abstracts;
using ch_15_identity.Entities;
using ch_15_identity.Entities.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace ch_15_identity.Services;

public class AuthenticationManager : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public AuthenticationManager(UserManager<User> userManager,IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<IdentityResult> RegisterUserAsync(UserDtoForRegister? userDtoForRegister)
    {
        if (userDtoForRegister == null)
        {
            throw new ArgumentNullException();
        }
        
        Validate(userDtoForRegister);
        
        var user = _mapper.Map<User>(userDtoForRegister);
        var result = await _userManager.CreateAsync(user, userDtoForRegister.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userDtoForRegister.Roles!);
        }
        
        return result;
    }
    
    private void Validate<T>(T item)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(item!);
        var isValid = Validator.TryValidateObject(item!, context, validationResults, true);

        if (!isValid)
        {
            var errors = string.Join(" , ", validationResults);
            throw new ValidationException(errors);
        }
    }
}