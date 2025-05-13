using System.ComponentModel.DataAnnotations;
using System.Text;
using ch_16_jwt.Entities;
using ch_16_jwt.Entities.Exceptions;
using ch_16_jwt.Repositories.Base_Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ch_16_jwt.Configuration;

// eklenti metotları static classlar içerisine yazılır.
// static bir sınıfın tüm üyeleri static olmalıdır.

public static class ConfigurationExtensions
{
    // bu metot sayesinde int a = 5 gibi bir ifade tanımlandıktan sonra a.ValidateInRange() kullanımı yapılabilir.
    public static void ValidateIdInRange(this int id)
    {
        if (!(id > 0 && id <= 1000))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID 1 ile 1000 arasında olmalıdır.");
        }
    }

    
    // bu metot sayesinde Program.cs içerisinde app.UseCustomExceptionHandler() kullanımı yapılabilir.
    // böylece Program.cs içerisi daha temiz bir hale gelir.
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        ValidationException => StatusCodes.Status422UnprocessableEntity,
                        ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
                        ArgumentException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                    }.ToString());
                }
            });
        });

    }


    public static IServiceCollection UseCustomAddCors(this IServiceCollection services)
    { 
        services.AddCors(options =>
        {
            options.AddPolicy("all", corsoptions =>
            {
                corsoptions.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
    
            options.AddPolicy("special", specialoptions =>
            {
                specialoptions.WithOrigins("http://localhost:8080/").AllowAnyMethod().AllowAnyHeader().AllowCredentials(); 
            });
    
        });

        return services;
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        // appsettings.json içindeki "JwtSettings" bölümünü okur
        var jwtSettings = configuration.GetSection("JwtSettings");

        // Token'ların imzalanmasında kullanılacak gizli anahtar
        var secretKey = jwtSettings["secretKey"];

        services.AddAuthentication(options =>
        {
            // Kimlik doğrulama yapılırken kullanılacak varsayılan şema: "Bearer"
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            // Yetkilendirme başarısız olduğunda devreye girecek varsayılan şema
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Token'ın hangi sunucu (issuer) tarafından üretildiği kontrol edilsin mi?
                ValidateIssuer = true,

                // Token hangi istemci (audience) için üretildi, kontrol edilsin mi?
                ValidateAudience = true,

                // Token'ın süresi dolmuş mu, kontrol edilsin mi?
                ValidateLifetime = true,

                // Token gerçekten bizim gizli anahtarımızla mı imzalanmış, kontrol edilsin mi?
                ValidateIssuerSigningKey = true,

                // Beklenen sunucu adresi (örneğin: https://example.com)
                ValidIssuer = jwtSettings["validIssuer"],

                // Beklenen istemci (örneğin: https://exampleclient.com)
                ValidAudience = jwtSettings["validAudience"],

                // Token'ın imzasını doğrulamak için kullanılan güvenlik anahtarı
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });
    }
    
        
   
}