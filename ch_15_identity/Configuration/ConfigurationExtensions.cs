using System.ComponentModel.DataAnnotations;
using ch_15_identity.Entities;
using ch_15_identity.Entities.Exceptions;
using ch_15_identity.Repositories.Base_Context;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;


namespace ch_15_identity.Configuration;

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

   
}