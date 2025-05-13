using ch_14_relations.Abstracts;
using ch_14_relations.Configuration;
using ch_14_relations.Entities.DTOs;
using ch_14_relations.Repositories;
using ch_14_relations.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseCustomAddCors();

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
app.UseCustomExceptionHandler();

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


app.Run();



