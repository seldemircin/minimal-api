namespace ch_14_relations.Entities;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; } // foreign key
    public Category Category { get; set; } // navigation prop
}