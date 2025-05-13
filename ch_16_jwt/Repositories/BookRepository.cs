using ch_16_jwt.Entities;
using ch_16_jwt.Repositories.Base_Context;
using Microsoft.EntityFrameworkCore;

namespace ch_16_jwt.Repositories;

public class BookRepository : RepositoryBase<Book>
{
    // repolar ile ilgili entity üzerindeki temel crud işlemleri gerçekleştirilir.
    // bu reponun kullanıldığı serviler içerisindeki kodlar kısalır ve kontrol daha kolay olur.
    

    public BookRepository(RepositoryContext context) : base(context)
    {
    }

    public override List<Book> GetAll()
    {
        return _context.Books.Include(b => b.Category).ToList(); // eager loading
    }

    public override Book? Get(int id)
    {
        return _context.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id); // eager loading
    }
    
}