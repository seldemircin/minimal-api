namespace ch_11_repo_in_use.Repositories;

public class BookRepository
{
    // ilgili servis içerisindeki kodların kısaltılmasını sağlar.
    
    private readonly RepositoryContext _context;

    public BookRepository(RepositoryContext context)
    {
        _context = context;
    }

    public List<Book> GetAll()
    {
        return _context.Books.ToList();
    }

    public Book? Get(int id)
    {
        return _context.Books.Find(id);
    }

    public void Add(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}