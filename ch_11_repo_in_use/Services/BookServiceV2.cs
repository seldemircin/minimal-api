using ch_11_repo_in_use.Abstracts;
using ch_11_repo_in_use.Repositories;

namespace ch_11_repo_in_use.Services;

public class BookServiceV2 : IBookService
{
    private readonly RepositoryContext _context;

    public BookServiceV2(RepositoryContext context)
    {
        _context = context;
    }

    public int Count => _context.Books.ToList().Count;
    public List<Book> GetBooks()
    {
        return _context.Books.ToList();
    }

    public Book? GetBookById(int id)
    {
        return _context.Books.Find(id);
    }

    public void AddBook(Book item)
    {
        _context.Books.Add(item);
        _context.SaveChanges();
    }

    public Book UpdateBook(int id, Book item)
    {
        var book = GetBookById(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        book.Title = item.Title;
        book.Price = item.Price;
        _context.SaveChanges();
        return item;
    }

    public void DeleteBookById(int id)
    {
        var book = GetBookById(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}