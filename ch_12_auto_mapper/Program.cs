using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ch_12_auto_mapper.Abstracts;
using ch_12_auto_mapper.Entities;
using ch_12_auto_mapper.Entities.DTOs;
using ch_12_auto_mapper.Entities.Exceptions;
using ch_12_auto_mapper.Repositories;
using ch_12_auto_mapper.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
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

// DI
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IBookService,BookServiceV3>();
builder.Services.AddDbContext<RepositoryContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("all");
app.UseHttpsRedirection();
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


app.MapGet("/api/books", (IBookService bookService) =>
{
    return bookService.Count > 0 ? Results.Ok(bookService.GetBooks()) : Results.NotFound();
});

app.MapGet("/api/books/{id:int}", (int id,IBookService bookService) =>
{
    var book = bookService.GetBookById(id);
    return book is null ? throw new BookNotFoundException(id) : Results.Ok(book);
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


app.Run();



