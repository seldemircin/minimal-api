namespace ch_15_identity.Entities.DTOs;

public record BookDtoForUpdate : BookDtoBase
{
    public int CategoryId { get; set; }
}