namespace ch_15_identity.Repositories.Base_Context;

public abstract class RepositoryBase<T>  where T : class
{
    // protected olarak tanımlanması sayesinde child repolardan da kullanılabilir hale gelir.
    protected readonly RepositoryContext _context;

    public RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }

    // standart olarak lazy loading olarak tanımlanan bu fonksiyonlar
    // istenen durumlarda child sınıflardan override edilerek özelleştirilebilir.
    
    public virtual T? Get(int id)
    {
        return _context.Set<T>().Find(id);
    }
    
    public virtual List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public virtual void Add(T item)
    {
        _context.Set<T>().Add(item);
        _context.SaveChanges();
    }

    public virtual void Update(T item)
    {
        _context.Set<T>().Update(item);
        _context.SaveChanges();
    }
    
    public virtual void Delete(T item)
    {
        _context.Set<T>().Remove(item);
        _context.SaveChanges();
    }
}