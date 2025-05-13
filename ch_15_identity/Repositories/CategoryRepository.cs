using ch_15_identity.Entities;
using ch_15_identity.Repositories.Base_Context;

namespace ch_15_identity.Repositories;

public class CategoryRepository : RepositoryBase<Category>
{
    // repolar ile ilgili entity üzerindeki temel crud işlemleri gerçekleştirilir.
    // bu reponun kullanıldığı serviler içerisindeki kodlar kısalır ve kontrolü daha kolay olur.

    public CategoryRepository(RepositoryContext context) : base(context)
    {
    }
}