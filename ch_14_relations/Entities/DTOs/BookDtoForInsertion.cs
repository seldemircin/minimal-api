namespace ch_14_relations.Entities.DTOs;

public record BookDtoForInsertion : BookDtoBase
{
    public int CategoryId { get; set; }
}