namespace ch_16_jwt.Entities.DTOs.Books;

public record BookDto : BookDtoBase
{
    public int Id { get; init; }
    
    public Category Category { get; set; } // navigation prop
}