using ch_11_repo_in_use.Abstracts;

namespace ch_11_repo_in_use.Services;

public class BookService : IBookService
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