using System.ComponentModel.DataAnnotations;

namespace ch_14_relations.Entities.DTOs;

public record BookDto : BookDtoBase
{
    public int Id { get; init; }
    
    public Category Category { get; set; } // navigation prop
}