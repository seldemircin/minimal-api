var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/books", () =>
{
    return Book.List.Count > 0 ? Results.Ok(Book.List) : Results.NotFound();
});
app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book =  Book.List.FirstOrDefault(b => b.Id == id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});
app.MapPost("/api/books", (Book? newBook) =>
{
    if (newBook == null)
    {
        return Results.BadRequest();
    }
    newBook.Id = Book.List.Max(b => b.Id) + 1;
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
        return Results.NotFound();
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
        return Results.NotFound();
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


class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    
    // Seed Data
    private static List<Book> _bookList = new List<Book>(){
        new Book()
        {
            Id = 1, Title = "Kitap 1", Price = 10
        },
        new Book()
        {
            Id = 2, Title = "Kitap 2", Price = 20
        },
        new Book()
        {
            Id = 3, Title = "Kitap 3", Price = 30
        }
    };
    public static List<Book> List => _bookList;
}