using ch_15_identity.Abstracts;
using ch_15_identity.Configuration;
using ch_15_identity.Entities.DTOs;
using ch_15_identity.Entities.DTOs.Users;
using ch_15_identity.Repositories;
using ch_15_identity.Repositories.Base_Context;
using ch_15_identity.Services;
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
builder.Services.AddAuthentication();
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

app.MapGet("/api/books", (IBookService bookService) =>
{
    return bookService.Count > 0 ? Results.Ok(bookService.GetBooks()) : Results.NotFound();
});

app.MapGet("/api/books/{id:int}", (int id,IBookService bookService) =>
{
    var book = bookService.GetBookById(id);
    return Results.Ok(book);
});

app.MapPost("/api/books", (BookDtoForInsertion? newBook,IBookService bookService) =>
{
    var book = bookService.AddBook(newBook);
    return Results.Created($"/api/books/{book.Id}", book);
});

app.MapPut("/api/books/{id:int}", (int id,BookDtoForUpdate? updateBook,IBookService bookService) =>
{
    var updatedBook = bookService.UpdateBook(id, updateBook);
    return Results.Ok(updatedBook);
});

app.MapDelete("/api/books/{id:int}", (int id,IBookService bookService) =>
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


app.Run();



