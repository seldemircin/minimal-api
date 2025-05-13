namespace ch_16_jwt.Entities;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    ICollection<Book> Books { get; set; } // navigation prop
}