using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

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
builder.Services.AddSingleton<IBookService,BookService>(); // DI kaydı yapılıyor.

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

app.MapGet("/api/error", () =>
{
    throw new Exception("An error has been occured!");
});
app.MapGet("/api/books", (IBookService bookService) =>
{
    return bookService.Count > 0 ? Results.Ok(bookService.GetBooks()) : Results.NotFound();
});
app.MapGet("/api/books/{id:int}", (int id,IBookService bookService) =>
{
    if (!(id > 0 && id <= 1000))
    {
        throw new ArgumentOutOfRangeException("1-1000");
    }
    
    var book = bookService.GetBookById(id);
    return book is null ? throw new BookNotFoundException(id) : Results.Ok(book);
});
app.MapPost("/api/books", (Book? newBook,IBookService bookService) =>
{
    if (newBook == null)
    {
        throw new ArgumentException("Eklenmek istenen book nesnesi null olmaz.");
    }

    var validationResult = new List<ValidationResult>();
    var context = new ValidationContext(newBook);
    var isValid = Validator.TryValidateObject(newBook, context, validationResult,true);

    if (isValid)
    {
        bookService.AddBook(newBook);
        return Results.Created($"/api/books/{newBook.Id}", newBook);
    }

    var error = string.Join(" , ", validationResult);
    throw new ValidationException(error);
});
app.MapPut("/api/books/{id:int}", (int id,Book? updateBook,IBookService bookService) =>
{
    if (!(id > 0 && id <= 1000))
    {
        throw new ArgumentOutOfRangeException("1-1000");
    }
    
    if (updateBook == null)
    {
        throw new ArgumentException("Kitap null olamaz.");
    }
    
    if (id != updateBook.Id)
    {
        throw new ArgumentException("Id ile gönderilen kitabın Id değeri eşit olmalıdır.");
    }

    var validationResult = new List<ValidationResult>();
    var context = new ValidationContext(updateBook);
    var isValid = Validator.TryValidateObject(updateBook, context, validationResult, true);

    if (isValid)
    {
        var updatedBook = bookService.UpdateBook(id, updateBook);
        return Results.Ok(updatedBook);
    }
    
    var error = string.Join(" , ", validationResult);
    throw new ValidationException(error);
    
});
app.MapDelete("/api/books/{id:int}", (int id,IBookService bookService) =>
{
    if (!(id > 0 && id <= 1000))
    {
        throw new ArgumentOutOfRangeException("1-1000");
    }
    
    bookService.DeleteBookById(id);
    return Results.NoContent();
});
app.MapGet("/api/books/search", (string? title,IBookService bookService) =>
{
    var books = string.IsNullOrEmpty(title) ? bookService.GetBooks() : bookService.GetBooks().Where(b => b.Title != null && b.Title.ToLower().Contains(title.ToLower())).ToList();
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
    [Required]
    public int Id { get; set; }
    [MinLength(2,ErrorMessage = "Min. len must be 2")]
    [MaxLength(25,ErrorMessage = "Max. len must be 25")]
    public string? Title { get; set; }
    [Range(10,100)]
    public decimal Price { get; set; }
}

interface IBookService
{
    int Count { get; } // readonly prop
    List<Book> GetBooks();
    Book? GetBookById(int id);
    void AddBook(Book item);
    Book UpdateBook(int id, Book item);
    void DeleteBookById(int id);
}

class BookService : IBookService
{
    private readonly List<Book> _books;

    public BookService()
    {
        _books = new List<Book>(){
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
    }

    public int Count => _books.Count;

    public List<Book> GetBooks() => _books;

    public Book? GetBookById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public void AddBook(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;
        _books.Add(book);
    }

    public Book UpdateBook(int id, Book updateBook)
    {
        var book = GetBookById(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        book.Title = updateBook.Title;
        book.Price = updateBook.Price;
        return book;
    }

    public void DeleteBookById(int id)
    {
        var book = GetBookById(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        _books.Remove(book);
    }
}