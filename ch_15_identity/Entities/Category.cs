namespace ch_15_identity.Entities;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    ICollection<Book> Books { get; set; } // navigation prop
}