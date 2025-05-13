namespace ch_15_identity.Entities.DTOs;

public record BookDtoForInsertion : BookDtoBase
{
    public int CategoryId { get; set; }
}