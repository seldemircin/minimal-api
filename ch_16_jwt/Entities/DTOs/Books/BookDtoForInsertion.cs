namespace ch_16_jwt.Entities.DTOs.Books;

public record BookDtoForInsertion : BookDtoBase
{
    public int CategoryId { get; set; }
}