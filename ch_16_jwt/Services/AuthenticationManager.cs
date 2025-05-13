using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ch_16_jwt.Abstracts;
using ch_16_jwt.Entities;
using ch_16_jwt.Entities.DTOs;
using ch_16_jwt.Entities.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ch_16_jwt.Services;

/// <summary>
/// Kimlik doğrulama ve yetkilendirme işlemlerini yöneten servis sınıfı.
/// Bu sınıf, kullanıcı kaydı, girişi, token oluşturma ve doğrulama işlemlerini gerçekleştirir.
/// Identity framework ve JWT (JSON Web Token) kullanarak güvenli kimlik doğrulama sağlar.
/// </summary>
public class AuthenticationManager : IAuthService
{
    private readonly UserManager<User> _userManager; // Kullanıcı yönetimi için Identity servisi
    private readonly IMapper _mapper; // DTO-Entity dönüşümleri için AutoMapper
    private User? _user; // Mevcut işlem yapılan kullanıcı
    private readonly IConfiguration _configuration; // Uygulama yapılandırma ayarları

    /// <summary>
    /// AuthenticationManager sınıfının constructor'ı.
    /// Dependency Injection ile gerekli servisleri alır.
    /// </summary>
    /// <param name="userManager">Kullanıcı yönetimi için Identity servisi</param>
    /// <param name="mapper">DTO-Entity dönüşümleri için AutoMapper</param>
    /// <param name="configuration">Uygulama yapılandırma ayarları</param>
    public AuthenticationManager(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Yeni kullanıcı kaydı oluşturur.
    /// Kullanıcı bilgilerini doğrular, şifreyi hashler ve veritabanına kaydeder.
    /// Başarılı kayıt durumunda kullanıcıya roller atar.
    /// </summary>
    /// <param name="userDtoForRegister">Kayıt için gerekli kullanıcı bilgileri</param>
    /// <returns>Kayıt işleminin sonucunu içeren IdentityResult</returns>
    /// <exception cref="ArgumentNullException">userDtoForRegister null olduğunda fırlatılır</exception>
    /// <exception cref="ValidationException">Validasyon hataları olduğunda fırlatılır</exception>
    public async Task<IdentityResult> RegisterUserAsync(UserDtoForRegister? userDtoForRegister)
    {
        // Null kontrolü
        if (userDtoForRegister == null)
        {
            throw new ArgumentNullException(nameof(userDtoForRegister),"Kayıt için gönderilen DTO null olamaz.");
        }
        
        // DTO validasyonu
        Validate(userDtoForRegister);
        
        // DTO'yu User entity'sine dönüştürme
        var user = _mapper.Map<User>(userDtoForRegister);
        
        // Kullanıcıyı veritabanına kaydetme
        var result = await _userManager.CreateAsync(user, userDtoForRegister.Password!);
        
        // Başarılı kayıt durumunda rolleri atama
        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userDtoForRegister.Roles!);
        }
        
        return result;
    }
    
    /// <summary>
    /// Kullanıcı giriş bilgilerini doğrular.
    /// Kullanıcı adı ve şifre kontrolü yapar.
    /// Başarılı doğrulama durumunda kullanıcı bilgilerini sınıf içinde saklar.
    /// </summary>
    /// <param name="userDtoForAuthentication">Giriş bilgileri</param>
    /// <returns>Doğrulama başarılı ise true, değilse false</returns>
    public async Task<bool> ValidateUserAsync(UserDtoForAuthentication? userDtoForAuthentication)
    {
        // Null kontrolü
        if (userDtoForAuthentication == null)
        {
            return false;
        }

        // Kullanıcı adına göre kullanıcıyı bulma
        _user = await _userManager.FindByNameAsync(userDtoForAuthentication.UserName!);
        
        // Kullanıcı bulunamadıysa false dön
        if (_user == null)
        {
            return false;
        }

        // Şifre doğrulama
        var result = await _userManager.CheckPasswordAsync(_user, userDtoForAuthentication.Password!);
        
        return result;
    }

    /// <summary>
    /// JWT (JSON Web Token) ve Refresh Token oluşturur.
    /// Kullanıcı bilgilerini ve rollerini token içine ekler.
    /// Token'ı imzalar ve yapılandırılmış süre ile oluşturur.
    /// </summary>
    /// <param name="populateExp">Token süresinin uzatılıp uzatılmayacağı</param>
    /// <returns>Access Token ve Refresh Token içeren TokenDto</returns>
    public async Task<TokenDto> CreateTokenAsync(bool populateExp)
    {
        // Kullanıcı claim'lerini alma
        var claims = await GetClaimsAsync();
        
        // Token imzalama bilgilerini oluşturma
        var signinCredentials = GetSigninCredentials();
        
        // Token seçeneklerini yapılandırma
        var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

        // Refresh token oluşturma
        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;

        // Token süresini ayarlama
        if (populateExp)
        {
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        // Kullanıcı bilgilerini güncelleme
        await _userManager.UpdateAsync(_user);

        // Token'ı string'e dönüştürme
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        // TokenDto oluşturma ve dönme
        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Refresh token oluşturur.
    /// Güvenli rastgele bir string üretir.
    /// </summary>
    /// <returns>Oluşturulan refresh token string'i</returns>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Eski token'ları yeniler.
    /// Geçersiz veya süresi dolmuş token'lar için yeni token çifti oluşturur.
    /// </summary>
    /// <param name="tokenDto">Eski token bilgileri</param>
    /// <returns>Yeni token çifti</returns>
    /// <exception cref="SecurityTokenException">Geçersiz veya süresi dolmuş token durumunda fırlatılır</exception>
    public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        // Eski token'dan kullanıcı bilgilerini alma
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        
        // Kullanıcıyı bulma
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
        
        // Token kontrolü
        if (user == null || 
            user.RefreshToken != tokenDto.RefreshToken || 
            user.RefreshTokenExpiryTime < DateTime.Now)
        {
            throw new SecurityTokenException("RefreshToken is invalid or expired");
        }
        
        // Kullanıcıyı güncelleme ve yeni token oluşturma
        _user = user;
        return await CreateTokenAsync(false);
    }

    /// <summary>
    /// Süresi dolmuş token'dan kullanıcı bilgilerini çıkarır.
    /// Token'ın geçerliliğini kontrol eder.
    /// </summary>
    /// <param name="token">Süresi dolmuş token</param>
    /// <returns>Token içindeki kullanıcı bilgileri</returns>
    /// <exception cref="SecurityTokenException">Geçersiz token durumunda fırlatılır</exception>
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // JWT ayarlarını alma
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["secretKey"];
        
        // Token doğrulama parametrelerini ayarlama
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
        
        // Token'ı doğrulama
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        
        // Token formatını kontrol etme
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || 
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        return principal;
    }

    /// <summary>
    /// JWT token seçeneklerini yapılandırır.
    /// Token'ın issuer, audience, süre ve imza bilgilerini ayarlar.
    /// </summary>
    /// <param name="signinCredentials">Token imzalama bilgileri</param>
    /// <param name="claims">Token içine eklenecek kullanıcı bilgileri</param>
    /// <returns>Yapılandırılmış JwtSecurityToken</returns>
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
    {
        // JWT ayarlarını configuration'dan alma
        var jwtSettings = _configuration.GetSection("JwtSettings");
        
        // Token seçeneklerini oluşturma
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expire"])),
            signingCredentials: signinCredentials);
        
        return tokenOptions;
    }

    /// <summary>
    /// Kullanıcının claim'lerini oluşturur.
    /// Kullanıcı adı ve rollerini claim olarak ekler.
    /// </summary>
    /// <returns>Kullanıcı claim'lerini içeren liste</returns>
    private async Task<List<Claim>> GetClaimsAsync()
    {
        // Temel claim'leri oluşturma
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _user!.UserName!),
        };

        // Kullanıcının rollerini alma
        var roles = await _userManager.GetRolesAsync(_user);

        // Her rol için bir claim ekleme
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    /// <summary>
    /// JWT token'ı imzalamak için gerekli bilgileri oluşturur.
    /// Configuration'dan secret key'i alır ve güvenli imzalama bilgilerini oluşturur.
    /// </summary>
    /// <returns>Token imzalama bilgileri</returns>
    private SigningCredentials GetSigninCredentials()
    {
        // JWT ayarlarını configuration'dan alma
        var jwtSettings = _configuration.GetSection("JwtSettings");
        
        // Secret key'i byte array'e dönüştürme
        var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!);
        
        // Güvenli key oluşturma
        var secret = new SymmetricSecurityKey(key);

        // İmzalama bilgilerini oluşturma
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    /// <summary>
    /// Generic bir validasyon mekanizması sağlar.
    /// DataAnnotations kullanarak veri doğrulaması yapar.
    /// </summary>
    /// <typeparam name="T">Validasyon yapılacak tip</typeparam>
    /// <param name="item">Validasyon yapılacak nesne</param>
    /// <exception cref="ValidationException">Validasyon hataları olduğunda fırlatılır</exception>
    private void Validate<T>(T item)
    {
        // Validasyon sonuçlarını tutacak liste
        var validationResults = new List<ValidationResult>();
        
        // Validasyon context'i oluşturma
        var context = new ValidationContext(item!);
        
        // Validasyon yapma
        var isValid = Validator.TryValidateObject(item!, context, validationResults,true);

        // Validasyon hataları varsa exception fırlatma
        if (!isValid)
        {
            var errors = string.Join(" , ", validationResults);
            throw new ValidationException(errors);
        }
    }
}