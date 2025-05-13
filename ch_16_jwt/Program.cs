using ch_16_jwt.Abstracts;
using ch_16_jwt.Configuration;
using ch_16_jwt.Entities;
using ch_16_jwt.Entities.DTOs;
using ch_16_jwt.Entities.DTOs.Books;
using ch_16_jwt.Entities.DTOs.Users;
using ch_16_jwt.Repositories;
using ch_16_jwt.Repositories.Base_Context;
using ch_16_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseCustomAddCors();

// DI
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IBookService,BookServiceV3>();
builder.Services.AddScoped<IAuthService, AuthenticationManager>();
builder.Services.AddDbContext<RepositoryContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("all");
app.UseHttpsRedirection();
app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/books", [Authorize(Roles = "Admin , User")](IBookService bookService) =>
{
    return bookService.Count > 0 ? Results.Ok(bookService.GetBooks()) : Results.NotFound();
});

app.MapGet("/api/books/{id:int}", (int id,IBookService bookService) =>
{
    var book = bookService.GetBookById(id);
    return Results.Ok(book);
});

app.MapPost("/api/books", [Authorize(Roles = "Admin")] (BookDtoForInsertion? newBook,IBookService bookService) =>
{
    var book = bookService.AddBook(newBook);
    return Results.Created($"/api/books/{book.Id}", book);
});

app.MapPut("/api/books/{id:int}", [Authorize(Roles = "Admin , User")] (int id,BookDtoForUpdate? updateBook,IBookService bookService) =>
{
    var updatedBook = bookService.UpdateBook(id, updateBook);
    return Results.Ok(updatedBook);
});

app.MapDelete("/api/books/{id:int}", [Authorize(Roles = "Admin")] (int id,IBookService bookService) =>
{
    bookService.DeleteBookById(id);
    return Results.NoContent();
});

app.MapGet("/api/books/search", (string? title,IBookService bookService) =>
{
    var books = string.IsNullOrEmpty(title) ? bookService.GetBooks() : bookService.GetBooks().Where(b => b.Title != null && b.Title.ToLower().Contains(title.ToLower())).ToList();
    return books.Any() ? Results.Ok(books) : Results.NotFound();
});

app.MapPost("/api/auth/register", async(UserDtoForRegister? newUser , IAuthService authService) =>
{
    var result = await authService.RegisterUserAsync(newUser);
    return result.Succeeded ? Results.Ok(result) : Results.BadRequest(result.Errors);
});

app.MapPost("/api/auth/login", async(UserDtoForAuthentication? userDtoForAuthentication,IAuthService authService) =>
{
    if (!await authService.ValidateUserAsync(userDtoForAuthentication))
    {
        return Results.Unauthorized(); // 401 
    }

    return Results.Ok(new
    {
        Token = await authService.CreateTokenAsync(true)
    });
});

app.MapPost("/api/auth/refresh", async (TokenDto tokenDto, IAuthService authService) =>
{
    var tokenDtoToReturn = await authService.RefreshTokenAsync(tokenDto);
    return Results.Ok(tokenDtoToReturn);
});

app.MapGet("/api/user", [Authorize] async (HttpContext httpContext, UserManager<User> userManager) =>
{
    var userName = httpContext.User.Identity?.Name;

    if (string.IsNullOrEmpty(userName))
    {
        return Results.Unauthorized();
    }
    
    var user = await userManager.FindByNameAsync(userName);
    
    if (user == null)
    {
        return Results.NotFound();
    }
    
    var roles = await userManager.GetRolesAsync(user);

    return Results.Ok(new
    {
        user.Id,
        user.UserName,
        user.Email,
        user.RefreshToken,
        Roles = roles
    });
});



app.Run();



