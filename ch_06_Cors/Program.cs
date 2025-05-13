using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    // all
    options.AddPolicy("all", corsoptions =>
    {
        corsoptions.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
    
    // special
    options.AddPolicy("special", specialoptions =>
    {
        specialoptions.WithOrigins("http://localhost:8080/").AllowAnyMethod().AllowAnyHeader().AllowCredentials(); 
    });
    
});

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

app.MapGet("/api/error", () =>
{
    throw new Exception("An error has been occured!");
});
app.MapGet("/api/books", () =>
{
    return Book.List.Count > 0 ? Results.Ok(Book.List) : Results.NotFound();
});
app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book =  Book.List.FirstOrDefault(b => b.Id == id);
    return book is null ? throw new BookNotFoundException(id) : Results.Ok(book);
});
app.MapPost("/api/books", (Book? newBook) =>
{
    if (newBook == null)
    {
        return Results.BadRequest();
    }
    newBook.Id = Book.List.Count + 1;
    Book.List.Add(newBook);
    return Results.Created($"/api/books/{newBook.Id}", newBook);
});
app.MapPut("/api/books/{id:int}", (int id,Book? updateBook) =>
{
    if (updateBook == null)
    {
        return Results.BadRequest();
    }
    var book = Book.List.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        throw new BookNotFoundException(id);
    }
    if (id != updateBook.Id)
    {
        return Results.BadRequest();
    }

    book.Price = updateBook.Price;
    book.Title = updateBook.Title;
    return Results.Ok(book);
});
app.MapDelete("/api/books/{id:int}", (int id) =>
{
    var book = Book.List.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        throw new BookNotFoundException(id);
    }

    Book.List.Remove(book);
    return Results.NoContent();
});
app.MapGet("/api/books/search", (string? title) =>
{
    var books = string.IsNullOrEmpty(title) ? Book.List : Book.List.Where(b => b.Title != null && b.Title.ToLower().Contains(title.ToLower())).ToList();
    return books.Any() ? Results.Ok(books) : Results.NotFound();
});


app.Run();

abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {
        
    }
}

sealed class BookNotFoundException : NotFoundException
{
    public BookNotFoundException(int id) : base($"The book with {id} could not be found.")
    {
        
    }
}
class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string AtOccured => DateTime.Now.ToLongDateString();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    
    // Seed Data
    private static List<Book> _bookList = new List<Book>(){
        new Book()
        {
            Id = 1, Title = "Book 1", Price = 10
        },
        new Book()
        {
            Id = 2, Title = "Book 2", Price = 20
        },
        new Book()
        {
            Id = 3, Title = "Book 3", Price = 30
        }
    };
    public static List<Book> List => _bookList;
}