using ch_15_identity.Entities;
using ch_15_identity.Repositories.Base_Context;
using Microsoft.EntityFrameworkCore;

namespace ch_15_identity.Repositories;

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