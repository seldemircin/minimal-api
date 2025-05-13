namespace ch_16_jwt.Entities.DTOs.Books;

public record BookDtoForUpdate : BookDtoBase
{
    public int CategoryId { get; set; }
}