namespace ch_15_identity.Entities.DTOs;

public record BookDto : BookDtoBase
{
    public int Id { get; init; }
    
    public Category Category { get; set; } // navigation prop
}