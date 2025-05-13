namespace ch_14_relations.Entities.DTOs;

public record BookDtoForUpdate : BookDtoBase
{
    public int CategoryId { get; set; }
}