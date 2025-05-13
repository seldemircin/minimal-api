using ch_11_repo_in_use.Abstracts;
using ch_11_repo_in_use.Repositories;

namespace ch_11_repo_in_use.Services;

public class BookServiceV3 : IBookService
{
    private readonly BookRepository _bookRepository;
    
    public BookServiceV3(BookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public int Count => _bookRepository.GetAll().Count;
    public List<Book> GetBooks() => _bookRepository.GetAll();
    public Book? GetBookById(int id) =>  _bookRepository.Get(id);
    public void AddBook(Book item) =>  _bookRepository.Add(item);
    public Book UpdateBook(int id, Book item)
    {
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        book.Title = item.Title;
        book.Price = item.Price;
        _bookRepository.Update(book);
        return item;
    }

    public void DeleteBookById(int id)
    {
        var book = _bookRepository.Get(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        _bookRepository.Delete(book);
    }
}